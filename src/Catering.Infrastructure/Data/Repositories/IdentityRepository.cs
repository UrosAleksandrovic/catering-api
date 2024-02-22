using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class IdentityRepository<T> : BaseCrudRepository<T, CateringDbContext>, IIdentityRepository<T> 
    where T : Identity
{
    public IdentityRepository(CateringDbContext dbContextFactory) 
        : base(dbContextFactory) { }

    public Task<T> GetByEmailAsync(string email)
        => _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Email == email);

    public async Task<T> GetByIdAsync(string id)
        => await _dbContext.Set<T>().FindAsync(id);
}
