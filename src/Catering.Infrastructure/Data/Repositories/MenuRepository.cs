using Catering.Application.Aggregates.Menus;
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

    public async Task<(List<Menu> menus, int totalCount)> GetFilteredAsync(MenusFilter menusFilter)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var queryableMenus = dbContext.Menus.AsQueryable();
        queryableMenus = ApplyFilters(menusFilter, queryableMenus);

        var results = await queryableMenus.ToListAsync();

        return new(results, await queryableMenus.CountAsync());
    }

    private IQueryable<Menu> ApplyFilters(MenusFilter menusFilter, IQueryable<Menu> queryableMenus)
    {
        queryableMenus.AsNoTracking();

        return queryableMenus
            .Skip((menusFilter.PageIndex - 1) * menusFilter.PageSize)
            .Take(menusFilter.PageSize);
    }
}
