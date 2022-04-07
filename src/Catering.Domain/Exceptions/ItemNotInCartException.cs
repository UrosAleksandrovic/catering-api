using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.ItemAggregate;
using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemNotInCartException : CateringException
{
    public ItemNotInCartException(Guid cartId, Guid itemId)
        : base($"Item ({cartId}) not found in ({itemId}), therefore it cannot be modified.") { }

    protected ItemNotInCartException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
        : base(serializationInfo, streamingContext) { }
}
