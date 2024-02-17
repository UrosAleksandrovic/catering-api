using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class IdentityRepository<T> : BaseCrudRepository<T, CateringDbContext>, IIdentityRepository<T> 
    where T : Identity
{
    public IdentityRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory) { }

    public async Task<T> GetByEmailAsync(string email)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var result = await dbContext.Set<T>().FirstOrDefaultAsync(e => e.Email == email);

        return result;
    }

    public async Task<T> GetByIdAsync(string id)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var result = await dbContext.Set<T>().FindAsync(id);

        return result;
    }
}
