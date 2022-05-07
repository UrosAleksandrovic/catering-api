using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.ItemAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ItemRepository : BaseCrudRepository<Item, CateringDbContext>, IItemRepository
{
    protected ItemRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory)
    {
    }

    public async Task<(List<Item> items, int totalCount)> GetFilteredAsync(ItemsFilter itemsFilter)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var queryableItems = dbContext.Items.AsQueryable();
        queryableItems = ApplyFilters(itemsFilter, queryableItems);

        return new(await queryableItems.ToListAsync(), await queryableItems.CountAsync());
    }

    public async Task<List<Item>> GetItemsFromCartAsync(string cartOwnerId)
    {
        //TODO: Test this query
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var cartItems = dbContext.Set<Cart>()
           .AsNoTracking()
           .Where(c => c.CustomerId == cartOwnerId)
           .SelectMany(c => c.Items);

        var result = await dbContext.Items.AsNoTracking().Join(
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
        var result = await dbContext.Items.Where(i => i.MenuId == menuId).ToListAsync();

        return result;
    }

    public async Task UpdateRangeAsync(IEnumerable<Item> items)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Items.UpdateRange(items);
        await dbContext.SaveChangesAsync();
    }

    private IQueryable<Item> ApplyFilters(ItemsFilter itemsFilter, IQueryable<Item> queryableItems)
    {
        queryableItems.AsNoTracking();

        if (itemsFilter.Categories != null && itemsFilter.Categories.Any())
            queryableItems = queryableItems.Where(i => itemsFilter.Categories.All(i.Categories.Contains));

        if (itemsFilter.TopPrice != null)
            queryableItems = queryableItems.Where(i => i.Price <= itemsFilter.TopPrice);

        if (itemsFilter.BottomPrice != null)
            queryableItems = queryableItems.Where(i => i.Price >= itemsFilter.BottomPrice);

        queryableItems = queryableItems.Where(i => i.MenuId == itemsFilter.MenuId);

        return queryableItems
            .Skip(itemsFilter.PageIndex * itemsFilter.PageSize)
            .Take(itemsFilter.PageSize);
    }
}
