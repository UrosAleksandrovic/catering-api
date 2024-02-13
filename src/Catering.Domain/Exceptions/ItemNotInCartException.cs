namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemNotInCartException : CateringException
{
    public ItemNotInCartException(Guid cartId, Guid itemId)
        : base($"Item ({itemId}) not found in ({cartId}), therefore it cannot be modified.") { }

    public ItemNotInCartException()
    { }

    public ItemNotInCartException(string message) : base(message)
    { }

    public ItemNotInCartException(string message, Exception innerException) : base(message, innerException)
    { }
}
