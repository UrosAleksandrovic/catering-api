using Catering.Application.Aggregates.Orders.Dtos;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderManagementAppService
{
    Task<long> PlaceOrderAsync(string customerId, CreateOrderDto createOrder);
    Task CancelAsync(long orderId);
    Task ConfirmAsync(long orderId);
}
