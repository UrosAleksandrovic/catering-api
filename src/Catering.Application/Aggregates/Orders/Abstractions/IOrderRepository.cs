using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Menu;
using Catering.Domain.Aggregates.Order;

namespace Catering.Application.Aggregates.Orders.Abstractions;

public interface IOrderRepository : IBaseCrudRepository<Order>
{
    Task UpdateOrderWithCustomerAsync(Customer customer, Order order);
    Task CreateOrderForCustomerAsync(Customer customer, Order order, Cart cart);
    Task<List<Order>> GetActiveOrdersByItemAsync(Guid itemId);
    Task<Menu> GetOrderMenuAsync(long orderId);
}
