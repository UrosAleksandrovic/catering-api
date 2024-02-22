using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Domain.Aggregates.Cart;
using Catering.Domain.Aggregates.Item;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ItemRepository : BaseCrudRepository<Item, CateringDbContext>, IItemRepository
{
    public ItemRepository(CateringDbContext dbContextFactory) 
        : base(dbContextFactory)
    { }

    public Task<Item> GetByMenuAndIdAsync(Guid menuId, Guid itemId)
        => _dbContext
            .Set<Item>()
            .Where(i => i.Id == itemId)
            .Where(i => i.MenuId == menuId)
            .SingleOrDefaultAsync();

    public async Task<List<Item>> GetItemsFromCartAsync(string cartOwnerId)
    {
        var cartItems = _dbContext.Set<Cart>()
           .Where(c => c.CustomerId == cartOwnerId)
           .SelectMany(c => c.Items);

        var result = await _dbContext.Set<Item>().AsNoTracking().Join(
            cartItems,
            item => item.Id,
            cartItem => cartItem.ItemId,
            (item, cartItem) => item)
            .ToListAsync();        

        return result;
    }

    public async Task<List<Item>> GetItemsFromMenuAsync(Guid menuId)
    {
        var result = await _dbContext
            .Set<Item>()
            .Where(i => i.MenuId == menuId)
            .ToListAsync();

        return result;
    }

    public async Task UpdateRangeAsync(IEnumerable<Item> items)
    {
        _dbContext.Set<Item>().UpdateRange(items);
        await _dbContext.SaveChangesAsync();
    }
}
