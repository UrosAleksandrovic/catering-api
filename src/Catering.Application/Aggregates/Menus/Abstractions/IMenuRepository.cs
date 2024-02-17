using Catering.Domain.Aggregates.Menu;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenuRepository : IBaseCrudRepository<Menu>
{
    Task<Menu> GetByContactIdAsync(string contactId);

    Task<(List<Menu> Menus, int TotalCount)> GetFilteredAsync(MenusFilter menusFilter);
}
