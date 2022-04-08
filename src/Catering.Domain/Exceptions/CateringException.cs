using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class CateringException : Exception
{
    public CateringException() { }

    public CateringException(string message) : base(message) { }

    public CateringException(string message, Exception innerException) : base(message, innerException) { }

    protected CateringException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
