using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Dtos.Menu;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenuManagementAppService
{
    Task<Guid> CreateAsync(CreateMenuDto createMenu);
    Task UpdateAsync(Guid id, UpdateMenuDto updateMenu);
    Task DeleteAsync(Guid id);
    Task<MenuInfoDto> GetByIdAsync(Guid id);
    Task<MenuInfoDto> GetByIdAsync(Guid id, string requestorId);
    Task<FilterResult<MenuInfoDto>> GetFilteredAsync(MenusFilter menusFilter);
}
