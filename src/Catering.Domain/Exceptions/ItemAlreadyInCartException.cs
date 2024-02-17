using Catering.Domain.ErrorCodes;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemAlreadyInCartException : CateringException
{ 
    public const string CustomMessage = "Cannot add item ({itemId}) in cart ({cartId}) because it is already added.";

    public ItemAlreadyInCartException(Guid itemId, Guid cartId)
        : base (CartErrorCodes.ITEM_ALREADY_IN_CART, CustomMessage)
    {
        Data.Add(nameof(cartId), cartId);
        Data.Add(nameof(itemId), itemId);
    }
}
