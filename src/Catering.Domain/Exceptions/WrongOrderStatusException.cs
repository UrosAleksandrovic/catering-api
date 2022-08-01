using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class WrongOrderStatusException : CateringException
{
    public WrongOrderStatusException(string action, long orderId) 
        : base($"Order ({orderId}) cannot perform action ({action}) because it is not in correct status") { }

    protected WrongOrderStatusException(SerializationInfo info, StreamingContext context) 
        : base(info, context) { }
}
