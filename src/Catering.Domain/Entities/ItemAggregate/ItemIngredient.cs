using System.Diagnostics.CodeAnalysis;

namespace Catering.Domain.Entities.ItemAggregate;

public class ItemIngredient : BaseEntity<string>
{
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
