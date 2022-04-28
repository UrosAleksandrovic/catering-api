﻿using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Entities.MenuAggregate;
using Catering.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class OrderRepository : BaseRepository<Order, CateringDbContext>, IOrderRepository
{
    protected OrderRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
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

    public async Task<(List<Order>, int)> GetFilteredAsync(OrderFilter filters)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var filterableOrders = ApplyFilters(filters, dbContext.Set<Order>());

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

    public async Task UpdateOrderWithCustomerAsync(Customer customer, Order order)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Update(customer);
        dbContext.Update(order);

        await dbContext.SaveChangesAsync();
    }

    private IQueryable<Order> ApplyFilters(OrderFilter itemsFilter, IQueryable<Order> queryableOrders)
    {
        queryableOrders.AsNoTracking();

        if (itemsFilter.CustomerId != null)
            queryableOrders = queryableOrders.Where(o => o.CustomerId == itemsFilter.CustomerId);

        return queryableOrders
            .Skip(itemsFilter.PageIndex * itemsFilter.PageSize)
            .Take(itemsFilter.PageSize);
    }
}