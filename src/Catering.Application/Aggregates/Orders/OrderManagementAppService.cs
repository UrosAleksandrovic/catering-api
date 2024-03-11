using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Notifications;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Application.Results;
using Catering.Application.Validation;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Item;
using Catering.Domain.Aggregates.Order;
using Catering.Domain.Builders;
using Catering.Domain.ErrorCodes;
using Catering.Domain.Services.Abstractions;
using MediatR;

namespace Catering.Application.Aggregates.Orders;

internal class OrderManagementAppService : IOrderManagementAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderingService _orderingService;
    private readonly IValidationProvider _validationProvider;
    private readonly IMediator _publisher;

    public OrderManagementAppService(
        IOrderRepository orderRepository,
        IOrderingService orderingService,
        IValidationProvider validationProvider,
        IMediator publisher)
    {
        _orderRepository = orderRepository;
        _orderingService = orderingService;
        _validationProvider = validationProvider;
        _publisher = publisher;
    }

    public async Task<Result> CancelAsync(long orderId) 
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == default)
            return Result.NotFound();

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.CancelOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);

        _ = _publisher.Publish(new OrderCanceled { CustomerId = customer.IdentityId, OrderId = order.Id });

        return Result.Success();
    }

    public async Task<Result> ConfirmAsync(long orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == default)
            return Result.NotFound();

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.ConfirmOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);
        _ = _publisher.Publish(new OrderConfirmed { CustomerId = customer.IdentityId, OrderId = order.Id });

        return Result.Success();
    }

    public async Task<Result<long>> PlaceOrderAsync(string customerId, CreateOrderDto createRequest)
    {
        if (await _validationProvider.ValidateModelAsync(createRequest) is var valRes && !valRes.IsSuccess)
            return Result.From<long>(valRes);

        if (string.IsNullOrWhiteSpace(customerId))
            return Result.ValidationError(IdentityErrorCodes.INVALID_CUSTOMER_ID);

        var cartOfOrder = await _publisher.Send(new GetCartFromCustomer(customerId));
        var cartItems = await _publisher.Send(new GetItemsForPlacingOrder(customerId));
        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = customerId });

        if (cartOfOrder == default || cartItems.Count == 0)
            return Result.ValidationError(CartErrorCodes.CART_IS_EMPTY);

        var order = PlaceOrder(createRequest, cartOfOrder, cartItems, customer);
        await _orderRepository.CreateOrderForCustomerAsync(customer, order, cartOfOrder);

        _ = _publisher.Publish(new OrderPlaced { CustomerId = customerId, OrderId = order.Id });
        return Result.Success(order.Id);
    }

    private Order PlaceOrder(
        CreateOrderDto createRequest,
        Cart cartOfOrder,
        List<Item> cartItems,
        Customer customer)
    {
        var orderBuilder = new OrderBuilder()
            .HasCart(cartOfOrder)
            .HasDateOfDelivery(createRequest.ExpectedTimeOfDelivery)
            .HasItems(cartItems);

        if (createRequest.HomeDeliveryInfo != default)
            orderBuilder.HasHomeDeliveryOption(
                createRequest.HomeDeliveryInfo.StreetAndHouse,
                createRequest.HomeDeliveryInfo.FloorAndApartment);


        return _orderingService.PlaceOrder(customer, orderBuilder);
    }
}
