using Catering.Domain.ErrorCodes;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemNotInCartException : CateringException
{
    public const string CustomMessage = "Item ({itemId}) not found in ({cartId}), therefore it cannot be modified.";

    public ItemNotInCartException(Guid itemId, Guid cartId)
        : base(CartErrorCodes.ITEM_NOT_IN_CART, CustomMessage)
    {
        Data.Add(nameof(cartId), cartId);
        Data.Add(nameof(itemId), itemId);
    }
}
