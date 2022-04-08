using Catering.Domain.Entities.CartAggregate;
using Catering.Domain.Entities.ItemAggregate;
using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemAlreadyInCartException : CateringException
{
    public ItemAlreadyInCartException(Guid cartId, Guid itemId)
        : base($"Cannot add item ({itemId}) in cart ({cartId}) because it is already added.") { }

    protected ItemAlreadyInCartException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
