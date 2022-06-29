using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Domain.Entities.MenuAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class MenuRepository : BaseCrudRepository<Menu, CateringDbContext>, IMenuRepository
{
    public MenuRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
        : base(dbContextFactory) { }

    public async Task<Menu> GetByContactIdAsync(string contactId)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Menus
            .AsNoTracking()
            .Where(m => m.Contact.IdentityId == contactId)
            .FirstOrDefaultAsync();
    }
}
