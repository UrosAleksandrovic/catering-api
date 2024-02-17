using System.Diagnostics.CodeAnalysis;

namespace Catering.Domain.Aggregates.Item;

public class ItemIngredient
{
    public string Id { get; private set; }
    public Guid ItemId { get; private set; }
    public string DisplayName { get; private set; }

    public ItemIngredient(Guid itemId, string displayName)
    {
        ItemId = itemId;
        Id = displayName.ToLowerInvariant();
        DisplayName = displayName;
    }
}

public class ItemIngredientComparator : IEqualityComparer<ItemIngredient>
{
    public bool Equals(ItemIngredient x, ItemIngredient y) => x.Id == y.Id;

    public int GetHashCode([DisallowNull] ItemIngredient obj) => obj.Id.GetHashCode();
}
