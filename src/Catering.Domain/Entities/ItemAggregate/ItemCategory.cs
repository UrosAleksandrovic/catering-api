using System.Diagnostics.CodeAnalysis;

namespace Catering.Domain.Entities.ItemAggregate;

public class ItemCategory
{
    public string Id { get; private set; }
    public Guid ItemId { get; private set; }
    public string DisplayName { get; private set; }

    public ItemCategory(Guid itemId, string displayName)
    {
        ItemId = itemId;
        Id = displayName.ToLowerInvariant();
        DisplayName = displayName;
    }
}

public class ItemCategoryComparator : IEqualityComparer<ItemCategory>
{
    public bool Equals(ItemCategory x, ItemCategory y) => x?.Id == y?.Id;

    public int GetHashCode([DisallowNull] ItemCategory obj) => obj.Id.GetHashCode();
}
