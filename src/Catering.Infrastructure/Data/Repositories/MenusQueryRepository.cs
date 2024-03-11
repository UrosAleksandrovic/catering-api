using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Dtos.Menu;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Aggregates.Menu;
using Catering.Infrastructure.EFUtility;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class MenusQueryRepository : IMenusQueryRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public MenusQueryRepository(
        IDbContextFactory<CateringDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<PageBase<MenuInfoDto>> GetPageAsync(MenusFilter menusFilter)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        var queryableMenus = dbContext
            .Set<Menu>()
            .OrderBy(m => m.Name);

        var results = await _mapper.ProjectTo<MenuInfoDto>(queryableMenus.Paginate(menusFilter)).ToListAsync();

        return new(results, await queryableMenus.CountAsync());
    }

    public async Task<PageBase<MenuContactDetailedInfoDto>> GetContactsAsync(MenusFilter menusFilter)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var queryableContacts = dbContext.Menus.AsNoTracking().Join(
            dbContext.Identities,
            m => m.Contact.IdentityId,
            i => i.Id,
            (menu, identity) => new { Menu = menu, Identity = identity })
            .Select(x => new MenuContactDetailedInfoDto
            {
                MenuId = x.Menu.Id,
                MenuName = x.Menu.Name,
                Email = x.Menu.Contact.Email,
                FullName = x.Identity != null ? new FullName(x.Identity.FullName) : null,
                IdentityId = x.Identity != null ? x.Identity.Id : null,
            }).OrderBy(c => c.MenuName);

        return new(await queryableContacts.Paginate(menusFilter).ToListAsync(), await queryableContacts.CountAsync());
    }

    public async Task<string> GetContactEmailAsync(Guid menuId)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext
            .Menus
            .AsNoTracking()
            .Where(m => m.Id == menuId)
            .Select(m => m.Contact.Email)
            .SingleOrDefaultAsync();
    }
}
