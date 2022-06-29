using Catering.Application.Aggregates.Orders.Dtos;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderManagementAppService
{
    Task<long> PlaceOrderAsync(string customerId, CreateOrderDto createOrder);
    Task<OrderInfoDto> GetByIdAsync(long id, string requestorId);
    Task<FilterResult<OrderInfoDto>> GetFilteredAsync(OrdersFilter ordersFilters, string requestorId);
    Task CancelAsync(long orderId);
    Task ConfirmAsync(long orderId);
}
