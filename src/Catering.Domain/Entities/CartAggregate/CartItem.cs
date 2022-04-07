using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.CartAggregate;

public class CartItem
{
    public string Note { get; private set; }
    public int Quantity { get; private set; }
    public Guid ItemId { get; private set; }

    private CartItem() { }

    public CartItem(Guid itemId, string note, int quantity = 1)
    {
        Guard.Against.NegativeOrZero(quantity);
        Guard.Against.Default(itemId);

        Note = note;
        Quantity = quantity;
        ItemId = itemId;
    }

    public void IncrementQuantity(int quantity = 1)
    {
        Guard.Against.NegativeOrZero(quantity);

        Quantity += quantity;
    }

    public void DecrementQuantity(int quantity = 1)
    {
        Guard.Against.NegativeOrZero(quantity);

        if (quantity >= Quantity)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Quantity -= quantity;
    }

    public void EditNote(string newNote) => Note = newNote;
}
