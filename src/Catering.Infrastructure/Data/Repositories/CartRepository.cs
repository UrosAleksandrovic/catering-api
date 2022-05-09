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
}
