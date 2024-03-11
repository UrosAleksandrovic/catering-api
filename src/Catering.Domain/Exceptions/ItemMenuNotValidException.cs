using Catering.Domain.ErrorCodes;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemMenuNotValidException : CateringException
{
    private const string CustomMessage = "Cart has menu ({menuId}) that does not align with item menu ({itemMenuId})";

    public ItemMenuNotValidException() : base(CartErrorCodes.CART_MENU_AND_ITEM_MISMATCH) { }

    public ItemMenuNotValidException(Guid cartMenuId, Guid itemMenuId)
        : base(CartErrorCodes.CART_MENU_AND_ITEM_MISMATCH, CustomMessage) 
    {
        Data.Add(nameof(cartMenuId), cartMenuId);
        Data.Add(nameof(itemMenuId), itemMenuId);
    }
}
