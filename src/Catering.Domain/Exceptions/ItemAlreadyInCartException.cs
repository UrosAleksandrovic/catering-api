namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemAlreadyInCartException : CateringException
{
    public ItemAlreadyInCartException(Guid cartId, Guid itemId)
        : base($"Cannot add item ({itemId}) in cart ({cartId}) because it is already added.") { }
}
