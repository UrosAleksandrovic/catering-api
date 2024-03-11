using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Menu;
using Catering.Domain.Aggregates.Order;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class OrdersRepository : BaseCrudRepository<Order, CateringDbContext>, IOrderRepository
{
    public OrdersRepository(CateringDbContext dbContext) 
        : base(dbContext)
    { }

    public async Task CreateOrderForCustomerAsync(Customer customer, Order order, Cart cart)
    {
        _dbContext.Update(customer);
        _dbContext.Add(order);
        _dbContext.Remove(cart);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Order>> GetActiveOrdersByItemAsync(Guid itemId)
    {
        var result = await _dbContext.Set<Order>()
            .AsNoTracking()
            .Where(o => o.Items.Any(i => i.ItemId == itemId))
            .ToListAsync();

        return result;
    }

    public async Task<Menu> GetOrderMenuAsync(long orderId)
    {
        var menu = await _dbContext.Set<Order>().AsNoTracking().Join(
                _dbContext.Set<Menu>(),
                order => order.MenuId,
                menu => menu.Id,
                (order, menu) => menu)
            .FirstOrDefaultAsync();

        return menu;
    }

    public async Task UpdateOrderWithCustomerAsync(Customer customer, Order order)
    {
        _dbContext.Update(customer);
        _dbContext.Update(order);

        await _dbContext.SaveChangesAsync();
    }
}
