using Ardalis.GuardClauses;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Entities.CartAggregate;

public class Cart : BaseEntity<Guid>
{
    public string CustomerId { get; private set; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyList<CartItem> Items => _items;

    private Cart() { }

    public Cart(string customerId)
    {
        Guard.Against.NullOrWhiteSpace(customerId);

        Id = Guid.NewGuid();
        CustomerId = customerId;
    }

    public void AddItem(Guid itemId, int quantity = 1, string note = null)
    {
        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem != default)
            throw new ItemAlreadyInCartException(Id, itemId);

        _items.Add(new CartItem(itemId, quantity, note));
    }

    public void IncrementItem(Guid itemId, int quantity = 1)
    {
        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        existingItem.IncrementQuantity(quantity);
    }

    public void DecrementOrDeleteItem(Guid itemId, int quantity = 1)
    {
        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        if (existingItem.Quantity <= quantity)
            _items.Remove(existingItem);
        else
            existingItem.DecrementQuantity(quantity);
    }

    public void AddOrEditNoteToItem(Guid itemId, string note)
    {
        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        existingItem.EditNote(note);
    }
}
