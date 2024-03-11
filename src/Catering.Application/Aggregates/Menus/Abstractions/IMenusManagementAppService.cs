using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Dtos.Menu;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenusManagementAppService
{
    Task<Result<Guid>> CreateAsync(CreateMenuDto createMenu);
    Task<Result> UpdateAsync(Guid id, UpdateMenuDto updateMenu);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<MenuInfoDto>> GetByIdAsync(Guid id, string requestorId);
}
