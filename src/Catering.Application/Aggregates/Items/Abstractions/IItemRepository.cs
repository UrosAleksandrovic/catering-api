using Catering.Domain.Aggregates.Item;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemRepository : IBaseCrudRepository<Item>
{
    Task UpdateRangeAsync(IEnumerable<Item> items);
    Task<List<Item>> GetItemsFromMenuAsync(Guid menuId);
    Task<List<Item>> GetItemsFromCartAsync(string cartOwnerId);
    Task<Item> GetByMenuAndIdAsync(Guid menuId, Guid itemId);
}
