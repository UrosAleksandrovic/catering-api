﻿using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ItemRepository : BaseCrudRepository<Item, CateringDbContext>, IItemRepository
{
    public ItemRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory)
    { }

    public async Task<(List<Item> items, int totalCount)> GetFilteredAsync(ItemsFilter itemsFilter)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var queryableItems = dbContext.Set<Item>().AsQueryable();
        queryableItems = ApplyFilters(itemsFilter, queryableItems);

        if (!itemsFilter.OrderBy.HasValue)
            return (await queryableItems.ToListAsync(), await queryableItems.CountAsync());

        var orderedQueryable = ApplyOrdering(itemsFilter, queryableItems);
        
        return (await orderedQueryable.ToListAsync(), await orderedQueryable.CountAsync());
    }

    public async Task<List<(Item item, int numOfOrders)>> GetMostOrderedFromTheMenuAsync(int top, Guid menuId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var orderItemsFromMenu = dbContext
            .Set<Order>()
            .AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.MenuId == menuId)
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ItemId)
            .Select(g => new
            {
                ItemId = g.Key,
                NumOfOrders = g.Count(),
            });

        var queryableItems = dbContext.Set<Item>().AsNoTracking().Join(
            orderItemsFromMenu,
            item => item.Id,
            orderItem => orderItem.ItemId,
            (item, orderItem) => new { Item = item, orderItem.NumOfOrders })
            .OrderByDescending(r => r.NumOfOrders)
            .Take(top);

        var queryResult = await queryableItems.ToListAsync();

        var result = queryResult.Select(i => (i.Item, i.NumOfOrders)).ToList();

        return result;
    }

    public async Task<List<Item>> GetItemsFromCartAsync(string cartOwnerId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var cartItems = dbContext.Set<Cart>()
           .AsNoTracking()
           .Where(c => c.CustomerId == cartOwnerId)
           .SelectMany(c => c.Items);

        var result = await dbContext.Set<Item>().AsNoTracking().Join(
            cartItems,
            item => item.Id,
            cartItem => cartItem.ItemId,
            (item, cartItem) => item)
            .ToListAsync();        

        return result;
    }

    public async Task<List<Item>> GetItemsFromMenuAsync(Guid menuId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var result = await dbContext
            .Set<Item>()
            .Where(i => i.MenuId == menuId)
            .ToListAsync();

        return result;
    }

    public async Task UpdateRangeAsync(IEnumerable<Item> items)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Set<Item>().UpdateRange(items);
        await dbContext.SaveChangesAsync();
    }

    private IQueryable<Item> ApplyFilters(ItemsFilter itemsFilter, IQueryable<Item> queryableItems)
    {
        queryableItems.AsNoTracking();

        //TODO: Move to separate table since filtering is not possible...
        //if (itemsFilter.Categories != null && itemsFilter.Categories.Any())
        //    queryableItems = queryableItems.Where(i => itemsFilter.Categories.Intersect(i.Categories).Any());

        if (itemsFilter.TopPrice != null)
            queryableItems = queryableItems.Where(i => i.Price <= itemsFilter.TopPrice);

        if (itemsFilter.BottomPrice != null)
            queryableItems = queryableItems.Where(i => i.Price >= itemsFilter.BottomPrice);

        queryableItems = queryableItems.Where(i => i.MenuId == itemsFilter.MenuId);

        return queryableItems
            .Skip((itemsFilter.PageIndex - 1) * itemsFilter.PageSize)
            .Take(itemsFilter.PageSize);
    }

    private IOrderedQueryable<Item> ApplyOrdering(ItemsFilter itemsFilter, IQueryable<Item> queryableItems)
    {

        return (itemsFilter?.OrderBy) switch
        {
            ItemsOrderBy.Price => queryableItems.OrderBy(i => i.Price),
            ItemsOrderBy.Name => queryableItems.OrderBy(i => i.Name),
            ItemsOrderBy.Rating => queryableItems.OrderBy(i => i.Ratings.Average(r => r.Rating)),
            _ => queryableItems.OrderBy(i => i.Name),
        };
    }
}
