namespace Catering.Domain.ErrorCodes;

public static class CartErrorCodes
{
    public const string CART_MENU_NOT_SET = nameof(CART_MENU_NOT_SET);
    public const string CART_MENU_AND_ITEM_MISMATCH = nameof(CART_MENU_AND_ITEM_MISMATCH);
    public const string ITEM_ALREADY_IN_CART = nameof(ITEM_ALREADY_IN_CART);
    public const string ITEM_NOT_IN_CART = nameof(ITEM_NOT_IN_CART);
}
