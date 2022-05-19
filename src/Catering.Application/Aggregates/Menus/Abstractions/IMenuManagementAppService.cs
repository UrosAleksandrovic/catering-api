using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Dtos.Menu;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenuManagementAppService
{
    public Task<Guid> CreateAsync(CreateMenuDto createMenu);
    public Task UpdateAsync(Guid id, UpdateMenuDto updateMenu);
    public Task DeleteAsync(Guid id);
    public Task<MenuInfoDto> GetByIdAsync(Guid id);
    public Task<MenuInfoDto> GetByIdAsync(Guid id, string requestorId);
}
