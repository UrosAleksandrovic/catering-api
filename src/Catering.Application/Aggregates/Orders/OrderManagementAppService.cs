using AutoMapper;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Notifications;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Order;
using Catering.Domain.Builders;
using Catering.Domain.ErrorCodes;
using Catering.Domain.Exceptions;
using Catering.Domain.Services.Abstractions;
using MediatR;

namespace Catering.Application.Aggregates.Orders;

internal class OrderManagementAppService : IOrderManagementAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderingService _orderingService;
    private readonly IMediator _publisher;
    private readonly IMapper _mapper;

    public OrderManagementAppService(
        IOrderRepository orderRepository,
        IOrderingService orderingService,
        IMediator publisher,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderingService = orderingService;
        _publisher = publisher;
        _mapper = mapper;
    }

    public async Task CancelAsync(long orderId) 
    {
        var order = await GetExistingByIdAsync(orderId);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.CancelOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);

        _ = _publisher.Publish(new OrderCanceled { CustomerId = customer.IdentityId, OrderId = order.Id });
    }

    public async Task ConfirmAsync(long orderId)
    {
        var order = await GetExistingByIdAsync(orderId);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.ConfirmOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);
        _ = _publisher.Publish(new OrderConfirmed { CustomerId = customer.IdentityId, OrderId = order.Id });
    }

    public async Task<long> PlaceOrderAsync(string customerId, CreateOrderDto createOrder)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentNullException(nameof(customerId));

        var cartOfOrder = await _publisher.Send(new GetCartFromCustomer(customerId));
        var cartItems = await _publisher.Send(new GetItemsForPlacingOrder(customerId));
        if (cartOfOrder == default || cartItems.Count == 0)
            throw new CateringException(CartErrorCodes.CART_IS_EMPTY);

        var orderBuilder = new OrderBuilder()
            .HasCart(cartOfOrder)
            .HasDateOfDelivery(createOrder.ExpectedTimeOfDelivery)
            .HasItems(cartItems);

        if (createOrder.HomeDeliveryInfo != default)
            orderBuilder.HasHomeDeliveryOption(
                createOrder.HomeDeliveryInfo.StreetAndHouse,
                createOrder.HomeDeliveryInfo.FloorAndApartment);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = customerId });

        var order = _orderingService.PlaceOrder(customer, orderBuilder);

        await _orderRepository.CreateOrderForCustomerAsync(customer, order, cartOfOrder);

        _ = _publisher.Publish(new OrderPlaced { CustomerId = customerId, OrderId = order.Id });
        return order.Id;
    }

    private async Task<Order> GetExistingByIdAsync(long odrerId)
    {
        var order = await _orderRepository.GetByIdAsync(odrerId);
        if (order == default)
            throw new KeyNotFoundException();

        return order;
    }
}
