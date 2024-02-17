using Ardalis.GuardClauses;

namespace Catering.Domain.Aggregates.Cart;

public class CartItem
{
    public Guid CartId { get; private set; }
    public string Note { get; private set; }
    public int Quantity { get; private set; }
    public Guid ItemId { get; private set; }

    private CartItem() { }

    public CartItem(Guid itemId, int quantity = 1, string note = null)
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

        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(quantity, Quantity);

        Quantity -= quantity;
    }

    public void EditNote(string newNote) => Note = newNote;
}
