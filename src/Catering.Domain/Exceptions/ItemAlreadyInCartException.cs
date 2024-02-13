namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemAlreadyInCartException(Guid cartId, Guid itemId) 
    : CateringException($"Cannot add item ({itemId}) in cart ({cartId}) because it is already added.")
{ }
