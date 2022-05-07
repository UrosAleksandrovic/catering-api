using Catering.Application.Aggregates.Orders.Dtos;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderManagementAppService
{
    Task<long> PlaceOrderAsync(CreateOrderDto createOrder);
    Task<OrderInfoDto> GetOrderByIdAsync(long id);
    Task<FilterResult<OrderInfoDto>> GetFilteredOrders(OrderFilter orderFilters);
    Task CancelOrderAsync(long id);
    Task ConfirmOrderAsync(long id);
}
