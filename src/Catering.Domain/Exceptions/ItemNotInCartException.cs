namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemNotInCartException(Guid cartId, Guid itemId) 
    : CateringException($"Item ({itemId}) not found in ({cartId}), therefore it cannot be modified.")
{ }
