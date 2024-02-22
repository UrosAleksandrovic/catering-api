using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Domain.Aggregates.Menu;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class MenusRepository : BaseCrudRepository<Menu, CateringDbContext>, IMenusRepository
{
    public MenusRepository(CateringDbContext dbContext)
        : base(dbContext) { }

    public Task<Menu> GetByContactIdAsync(string contactId)
        => _dbContext.Menus
            .AsNoTracking()
            .Where(m => m.Contact.IdentityId == contactId)
            .FirstOrDefaultAsync();
}
