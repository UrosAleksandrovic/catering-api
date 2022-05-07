using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Catering.Domain.Entities.OrderAggregate;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderRepository : IBaseCrudRepository<Order>
{
    Task UpdateOrderWithCustomerAsync(Customer customer, Order order);
    Task CreateOrderForCustomerAsync(Customer customer, Order order);
    Task<List<Order>> GetActiveOrdersByItemAsync(Guid itemId);
    Task<Menu> GetOrderMenuAsync(long orderId);
    Task<(List<Order>, int)> GetFilteredAsync(OrderFilter filters);
}
