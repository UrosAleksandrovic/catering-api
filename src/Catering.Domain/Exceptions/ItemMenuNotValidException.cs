using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class ItemMenuNotValidException : CateringException
{
    public ItemMenuNotValidException() : base() { }

    public ItemMenuNotValidException(Guid cartMenuId, Guid itemMenuId)
        : base($"Cart has menu ({cartMenuId}) that does not align with item menu ({itemMenuId})") { }
}
