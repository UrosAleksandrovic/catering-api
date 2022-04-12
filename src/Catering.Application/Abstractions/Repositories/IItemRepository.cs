using Catering.Application.Dtos.Item;
using Catering.Application.Filters;
using Catering.Domain.Entities.ItemAggregate;

namespace Catering.Application.Abstractions.Repositories;

public interface IItemRepository : IBaseRepository<Item>
{
    Task<FilterResult<Item>> GetFilteredAsync(ItemsFilter itemsFilter);
}
