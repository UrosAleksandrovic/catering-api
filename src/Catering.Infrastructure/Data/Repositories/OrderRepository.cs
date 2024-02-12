using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Catering.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class OrderRepository : BaseCrudRepository<Order, CateringDbContext>, IOrderRepository
{
    public OrderRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory)
    { }

    public async Task CreateOrderForCustomerAsync(Customer customer, Order order)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Update(customer);
        dbContext.Add(order);

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Order>> GetActiveOrdersByItemAsync(Guid itemId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var result = await dbContext.Set<Order>()
            .AsNoTracking()
            .Where(o => o.Items.Any(i => i.ItemId == itemId))
            .ToListAsync();

        return result;
    }

    public async Task<(List<Order>, int)> GetFilteredAsync(OrdersFilter filters)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var filterableOrders = ApplyFilters(filters, dbContext.Set<Order>());
        if (!filters.OrderBy.HasValue)
            return (await filterableOrders.ToListAsync(), await filterableOrders.CountAsync());

        filterableOrders = ApplyOrdering(filters, filterableOrders);

        return (await filterableOrders.ToListAsync(), await filterableOrders.CountAsync());
    }

    public async Task<Menu> GetOrderMenuAsync(long orderId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var menu = await dbContext.Set<Order>().AsNoTracking().Join(
                dbContext.Set<Menu>(),
                order => order.MenuId,
                menu => menu.Id,
                (order, menu) => menu)
            .FirstOrDefaultAsync();

        return menu;
    }

    public async Task<(List<Order>, int)> GetOrdersForCustomerAsync(OrdersFilter filters)
    {
        if (filters.CustomerId == default)
            return (new List<Order>(), 0);

        return await GetFilteredAsync(filters);
    }

    public async Task<(List<Order>, int)> GetOrdersForMenuAsync(OrdersFilter filters)
    {
        if (filters.MenuId == default)
            return (new List<Order>(), 0);

        return await GetFilteredAsync(filters);
    }

    public async Task UpdateOrderWithCustomerAsync(Customer customer, Order order)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Update(customer);
        dbContext.Update(order);

        await dbContext.SaveChangesAsync();
    }

    private IQueryable<Order> ApplyFilters(OrdersFilter filters, IQueryable<Order> query)
    {
        query.AsNoTracking();

        query = query.Include(e => e.Items);

        if (filters.CustomerId != null)
            query = query.Where(o => o.CustomerId == filters.CustomerId);

        if (filters.MenuId != null)
            query = query.Where(o => o.MenuId == filters.MenuId);

        if (filters.TopPrice != null)
            query = query.Where(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity) <=  filters.TopPrice);

        if (filters.BottomPrice != null)
            query = query.Where(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity) >= filters.BottomPrice);

        if (filters.Statuses != null && filters.Statuses.Any())
            query = query.Where(o => filters.Statuses.Contains(o.Status));

        if (filters.DeliveredOn.HasValue)
            query = query.Where(o => o.ExpectedOn.DayOfYear == filters.DeliveredOn.Value.DayOfYear)
                                             .Where(o => o.ExpectedOn.Year == filters.DeliveredOn.Value.Year);

        if (filters.IsHomeDelivery.HasValue && filters.IsHomeDelivery.Value)
            query = query.Where(o => o.HomeDeliveryInfo != null);

        return query
            .Skip((filters.PageIndex - 1) * filters.PageSize)
            .Take(filters.PageSize);
    }

    private IOrderedQueryable<Order> ApplyOrdering(OrdersFilter filters, IQueryable<Order> query)
    {
        return filters switch
        {
            { OrderBy: OrdersOrderBy.TotalPrice, IsOrderByDescending: false } => query.OrderBy(i => i.TotalPrice),
            { OrderBy: OrdersOrderBy.TotalPrice, IsOrderByDescending: true } => query.OrderByDescending(i => i.TotalPrice),

            { OrderBy: OrdersOrderBy.Status, IsOrderByDescending: false } => query.OrderBy(i => i.Status),
            { OrderBy: OrdersOrderBy.Status, IsOrderByDescending: true } => query.OrderByDescending(i => i.Status),

            { OrderBy: OrdersOrderBy.ExpectedOn, IsOrderByDescending: false } => query.OrderBy(i => i.ExpectedOn),
            { OrderBy: OrdersOrderBy.ExpectedOn, IsOrderByDescending: true } => query.OrderByDescending(i => i.ExpectedOn),

            _ => query.OrderByDescending(i => i.ExpectedOn),
        };
    }
}
