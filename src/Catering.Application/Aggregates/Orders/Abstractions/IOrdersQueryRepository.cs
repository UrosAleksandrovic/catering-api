using Catering.Application.Aggregates.Orders.Dtos;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrdersQueryRepository
{
    Task<OrderInfoDto> GetByIdAsync(long id);
    Task<PageBase<OrderInfoDto>> GetFilteredAsync(OrdersFilter filters);
    Task<PageBase<OrderInfoDto>> GetOrdersForMenuAsync(OrdersFilter filters);
    Task<PageBase<OrderInfoDto>> GetOrdersForCustomerAsync(OrdersFilter filters);
}
