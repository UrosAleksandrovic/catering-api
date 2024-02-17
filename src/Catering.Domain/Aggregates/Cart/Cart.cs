using Ardalis.GuardClauses;
using Catering.Domain.ErrorCodes;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Aggregates.Cart;

public class Cart
{
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; }
    public Guid? MenuId { get; private set; }

    private readonly List<CartItem> _items = [];
    public IReadOnlyList<CartItem> Items => _items;

    private Cart() { }

    public Cart(string customerId)
    {
        Guard.Against.NullOrWhiteSpace(customerId);

        Id = Guid.NewGuid();
        CustomerId = customerId;
    }

    public void AddItem(Guid menuId, Guid itemId, int quantity = 1, string note = null)
    {
        CheckIfMenuIsValid(menuId, itemId);

        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem != default)
            throw new ItemAlreadyInCartException(Id, itemId);

        if (_items.Count == 0)
            MenuId = menuId;

        _items.Add(new CartItem(itemId, quantity, note));
    }

    public void IncrementItem(Guid menuId, Guid itemId, int quantity = 1)
    {
        CheckIfMenuIsValid(menuId, itemId);

        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        existingItem.IncrementQuantity(quantity);
    }

    public void DecrementOrDeleteItem(Guid menuId, Guid itemId, int quantity = 1)
    {
        CheckIfMenuIsValid(menuId, itemId);

        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        if (existingItem.Quantity <= quantity)
            RemoveItem(existingItem);
        else
            existingItem.DecrementQuantity(quantity);
    }

    public void RemoveItem(Guid menuId, Guid itemId)
    {
        CheckIfMenuIsValid(menuId, itemId);

        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        RemoveItem(existingItem);
    }

    public void AddOrEditNoteToItem(Guid menuId, Guid itemId, string note)
    {
        CheckIfMenuIsValid(menuId, itemId);

        var existingItem = _items.SingleOrDefault(x => x.ItemId == itemId);
        if (existingItem == default)
            throw new ItemNotInCartException(Id, itemId);

        existingItem.EditNote(note);
    }

    private void RemoveItem(CartItem itemToDelete)
    {
        _items.Remove(itemToDelete);

        if (_items.Count == 0)
            MenuId = null;
    }

    private void CheckIfMenuIsValid(Guid menuId, Guid itemId)
    {
        var cartHasItems = _items.Count != 0;
        if (cartHasItems && !MenuId.HasValue)
            throw new CateringException(CartErrorCodes.CART_MENU_NOT_SET);

        if (cartHasItems && MenuId != menuId)
            throw new ItemMenuNotValidException(MenuId.Value, itemId);
    }
}
