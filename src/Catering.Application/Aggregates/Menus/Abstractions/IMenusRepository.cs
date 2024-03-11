using Catering.Domain.Aggregates.Menu;

namespace Catering.Application.Aggregates.Menus.Abstractions;

public interface IMenusRepository : IBaseCrudRepository<Menu>
{
    Task<Menu> GetByContactIdAsync(string contactId);
}
