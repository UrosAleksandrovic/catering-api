using Catering.Application.Aggregates.Orders.Dtos;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderManagementAppService
{
    Task<Result<long>> PlaceOrderAsync(string customerId, CreateOrderDto createOrder);
    Task<Result> CancelAsync(long orderId);
    Task<Result> ConfirmAsync(long orderId);
}
