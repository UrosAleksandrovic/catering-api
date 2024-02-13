namespace Catering.Domain.Exceptions;

[Serializable]
public class CateringException : Exception
{
    public CateringException() { }

    public CateringException(string message) : base(message) { }

    public CateringException(string message, Exception innerException) : base(message, innerException) { }
}
