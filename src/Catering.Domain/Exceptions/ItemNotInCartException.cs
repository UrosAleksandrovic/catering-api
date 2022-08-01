using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemNotInCartException : CateringException
{
    public ItemNotInCartException(Guid cartId, Guid itemId)
        : base($"Item ({itemId}) not found in ({cartId}), therefore it cannot be modified.") { }

    protected ItemNotInCartException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
        : base(serializationInfo, streamingContext) { }
}
