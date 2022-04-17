using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Dtos.Menu;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenuManagementAppService
{
    public Task<Guid> CreateAsync(CreateMenuDto createMenu);
    public Task<Guid> UpdateAsync(Guid id, CreateMenuDto updateMenu);
    public Task DeleteAsync(Guid id);
    public Task<MenuInfoDto> GetByIdAsync(Guid id);
}
