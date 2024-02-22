using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Dtos.Menu;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenusQueryRepository
{
    Task<PageBase<MenuInfoDto>> GetPageAsync(MenusFilter menusFilter);
    Task<PageBase<MenuContactDetailedInfoDto>> GetContactsAsync(MenusFilter menusFilter);
}
