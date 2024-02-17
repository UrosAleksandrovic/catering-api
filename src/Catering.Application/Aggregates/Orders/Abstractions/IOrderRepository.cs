using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Menu;
using Catering.Domain.Aggregates.Order;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderRepository : IBaseCrudRepository<Order>
{
    Task UpdateOrderWithCustomerAsync(Customer customer, Order order);
    Task CreateOrderForCustomerAsync(Customer customer, Order order);
    Task<List<Order>> GetActiveOrdersByItemAsync(Guid itemId);
    Task<Menu> GetOrderMenuAsync(long orderId);
    Task<(List<Order>, int)> GetFilteredAsync(OrdersFilter filters);
    Task<(List<Order>, int)> GetOrdersForMenuAsync(OrdersFilter filters);
    Task<(List<Order>, int)> GetOrdersForCustomerAsync(OrdersFilter filters);
}
