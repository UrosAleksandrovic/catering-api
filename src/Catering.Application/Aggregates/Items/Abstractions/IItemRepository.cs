using Catering.Domain.Entities.ItemAggregate;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemRepository : IBaseRepository<Item>
{
    Task UpdateRangeAsync(IEnumerable<Item> item);
    Task<FilterResult<Item>> GetFilteredAsync(ItemsFilter itemsFilter);
    Task<List<Item>> GetItemsFromMenuAsync(Guid menuId);
    Task<List<Item>> GetItemsFromCartAsync(string cartOwnerId);
}
