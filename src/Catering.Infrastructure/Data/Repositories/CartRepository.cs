using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Domain.Entities.CartAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CartRepository : BaseCrudRepository<Cart, CateringDbContext>, ICartRepository
{
    public CartRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory) { }

    public async Task DeleteAsync(string customerId)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var entity = await dbContext
            .Set<Cart>()
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (entity != null)
        {
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<Cart> GetByCustomerIdAsync(string customerId)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var entity = await dbContext
            .Set<Cart>()
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        return entity;
    }

    public async Task CleanUpDeletedItemsAsync(Cart cart)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var entity = await dbContext
            .Set<Cart>()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(e => e.Id == cart.Id);

        var itemsToBeDeleted = entity.Items.Where(i => !cart.Items.Any(ci => ci.ItemId == i.ItemId));

        if (itemsToBeDeleted.Any())
        {
            dbContext.RemoveRange(itemsToBeDeleted);
            await dbContext.SaveChangesAsync();
        }
    }
}
