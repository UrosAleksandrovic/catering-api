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
    {
    }

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

    private IQueryable<Order> ApplyFilters(OrdersFilter ordersFilter, IQueryable<Order> queryableOrders)
    {
        queryableOrders.AsNoTracking();

        queryableOrders = queryableOrders.Include(e => e.Items);

        if (ordersFilter.CustomerId != null)
            queryableOrders = queryableOrders.Where(o => o.CustomerId == ordersFilter.CustomerId);

        if (ordersFilter.MenuId != null)
            queryableOrders = queryableOrders.Where(o => o.MenuId == ordersFilter.MenuId);

        if (ordersFilter.TopPrice != null)
            queryableOrders = queryableOrders.Where(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity) <=  ordersFilter.TopPrice);

        if (ordersFilter.BottomPrice != null)
            queryableOrders = queryableOrders.Where(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity) >= ordersFilter.BottomPrice);

        if (ordersFilter.Statuses != null && ordersFilter.Statuses.Any())
            queryableOrders = queryableOrders.Where(o => ordersFilter.Statuses.Contains(o.Status));

        if (ordersFilter.DeliveredOn.HasValue)
            queryableOrders = queryableOrders.Where(o => o.ExpectedOn.DayOfYear == ordersFilter.DeliveredOn.Value.DayOfYear)
                                             .Where(o => o.ExpectedOn.Year == ordersFilter.DeliveredOn.Value.Year);

        if (ordersFilter.IsHomeDelivery.HasValue && ordersFilter.IsHomeDelivery.Value)
            queryableOrders = queryableOrders.Where(o => o.HomeDeliveryInfo != null);

        return queryableOrders
            .Skip((ordersFilter.PageIndex - 1) * ordersFilter.PageSize)
            .Take(ordersFilter.PageSize);
    }

    private IOrderedQueryable<Order> ApplyOrdering(OrdersFilter ordersFilter, IQueryable<Order> queryableOrders)
    {
        return (ordersFilter?.OrderBy) switch
        {
            OrdersOrderBy.TotalPrice => queryableOrders.OrderBy(o => o.Items.Sum(i => i.PriceSnapshot * i.Quantity)),
            OrdersOrderBy.Status => queryableOrders.OrderBy(o => o.Status),
            OrdersOrderBy.ExpectedOn => queryableOrders.OrderBy(o => o.ExpectedOn),
            _ => queryableOrders.OrderByDescending(o => o.Id),
        };
    }
}
