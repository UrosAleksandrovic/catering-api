using System.Runtime.Serialization;

namespace Catering.Domain.Exceptions;

[Serializable]
public class RestourantNotAllowedActionException : ActionNotAllowedException
{
    public RestourantNotAllowedActionException() : base() { }

    protected RestourantNotAllowedActionException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
