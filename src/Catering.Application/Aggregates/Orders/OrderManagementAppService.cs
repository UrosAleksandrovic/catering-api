using AutoMapper;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Builders;
using Catering.Domain.Entities.OrderAggregate;
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

    public async Task CancelOrderAsync(long id)
    {
        var order = await GetExistingByIdAsync(id);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.CancelOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);
    }

    public async Task ConfirmOrderAsync(long id)
    {
        var order = await GetExistingByIdAsync(id);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.ConfirmOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);
        await _publisher.Publish(new OrderConfirmed { CustomerId = customer.Id, OrderId = order.Id });
    }

    public async Task<FilterResult<OrderInfoDto>> GetFilteredOrders(OrderFilter orderFilters)
    {
        var orders = await _orderRepository.GetAsync()
    }

    public async Task<OrderInfoDto> GetOrderByIdAsync(long id)
    {
        var order = await GetExistingByIdAsync(id);

        return _mapper.Map<OrderInfoDto>(order);
    }

    public async Task<long> PlaceOrderAsync(CreateOrderDto createOrder)
    {
        var cartOfOrder = await _publisher.Send(new GetCartFromCustomer { CustomerId = createOrder.CustomerId });

        var orderBuilder = new OrderBuilder()
            .HasCart(cartOfOrder)
            .HasDateOfDelivery(createOrder.ExpectedTimeOfDelivery)
            .HasItems(await _publisher.Send(new GetItemsForPlacingOrder { CustomerId = cartOfOrder.CustomerId }));

        if (createOrder.HomeDeliveryInfo != default)
            orderBuilder.HasHomeDeliveryOption(
                createOrder.HomeDeliveryInfo.StreetAndHouse,
                createOrder.HomeDeliveryInfo.FloorAndApartment);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = createOrder.CustomerId });

        var order = _orderingService.PlaceOrder(customer, orderBuilder);

        await _orderRepository.CreateOrderForCustomerAsync(customer, order);

        await _publisher.Send(new OrderPlaced { CustomerId = createOrder.CustomerId, OrderId = order.Id});
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
