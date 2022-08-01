using AutoMapper;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Aggregates.Orders.Notifications;
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

    public async Task CancelAsync(long orderId) 
    {
        var order = await GetExistingByIdAsync(orderId);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.CancelOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);

        _ = _publisher.Publish(new OrderCanceled { CustomerId = customer.Id, OrderId = order.Id });
    }

    public async Task ConfirmAsync(long orderId)
    {
        var order = await GetExistingByIdAsync(orderId);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = order.CustomerId });

        _orderingService.ConfirmOrder(customer, order);
        await _orderRepository.UpdateOrderWithCustomerAsync(customer, order);
        _ = _publisher.Publish(new OrderConfirmed { CustomerId = customer.Id, OrderId = order.Id });
    }

    public async Task<FilterResult<OrderInfoDto>> GetFilteredAsync(OrdersFilter orderFilters, string requestorId)
    {
        var orders = await GetFilteredOrdersBasedOnRequestorAsync(orderFilters, requestorId);

        return new FilterResult<OrderInfoDto>
        {
            PageIndex = orderFilters.PageIndex,
            PageSize = orderFilters.PageSize,
            Result = _mapper.Map<IEnumerable<OrderInfoDto>>(orders.Item1),
            TotalNumberOfPages = orders.Item2,
        };
    }

    public async Task<OrderInfoDto> GetByIdAsync(long id, string requestorId)
    {
        var order = await GetExistingByIdAsync(id);

        var customerRequest = new GetOrderCustomer { CustomerId = requestorId };
        var customer = await _publisher.Send(customerRequest);

        if (customer.IsCompanyEmployee && order.CustomerId != customer.Id)
            return null;

        return _mapper.Map<OrderInfoDto>(order);
    }

    public async Task<long> PlaceOrderAsync(string customerId, CreateOrderDto createOrder)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentNullException(nameof(customerId));

        var cartOfOrder = await _publisher.Send(new GetCartFromCustomer { CustomerId = customerId });
        if (cartOfOrder == default || !cartOfOrder.Items.Any())
            throw new ArgumentException("Customer does not have cart.");

        var orderBuilder = new OrderBuilder()
            .HasCart(cartOfOrder)
            .HasDateOfDelivery(createOrder.ExpectedTimeOfDelivery)
            .HasItems(await _publisher.Send(new GetItemsForPlacingOrder { CustomerId = customerId }));

        if (createOrder.HomeDeliveryInfo != default)
            orderBuilder.HasHomeDeliveryOption(
                createOrder.HomeDeliveryInfo.StreetAndHouse,
                createOrder.HomeDeliveryInfo.FloorAndApartment);

        var customer = await _publisher.Send(new GetOrderCustomer { CustomerId = customerId });

        var order = _orderingService.PlaceOrder(customer, orderBuilder);

        await _orderRepository.CreateOrderForCustomerAsync(customer, order);

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

    private async Task<(List<Order>, int)> GetFilteredOrdersBasedOnRequestorAsync(OrdersFilter orderFilters, string requestorId)
    {
        var identityRequest = new GetIdentityWithId { IdentityId = requestorId };
        var requestorIdentity = await _publisher.Send(identityRequest);

        if (requestorIdentity.IsCompanyEmployee && !requestorIdentity.IsAdministrator)
        {
            var newFilter = new OrdersFilter(orderFilters)
            {
                CustomerId = requestorIdentity.Id
            };
            return await _orderRepository.GetOrdersForCustomerAsync(newFilter);
        }

        if (requestorIdentity.IsRestourantEmployee)
        {
            var newFilter = new OrdersFilter(orderFilters)
            {
                MenuId = await _publisher.Send(new GetMenuWithContactId { ContactId = requestorId })
            };
            return await _orderRepository.GetOrdersForMenuAsync(newFilter);
        }

        return await _orderRepository.GetFilteredAsync(orderFilters);
    }
}
