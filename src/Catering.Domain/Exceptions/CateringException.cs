namespace Catering.Domain.Exceptions;

[Serializable]
public class CateringException : Exception
{
    public string ErrorCode { get; set; }

    public CateringException() { }

    public CateringException(string errorCode) : base() 
    { 
        ErrorCode = errorCode;
    }

    public CateringException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public CateringException(string errorCode, Exception innerException)
        : base(innerException.Message, innerException)
    {
        ErrorCode = errorCode;
    }

    public CateringException(string errorCode, string message, Exception innerException) 
        : base(message, innerException) 
    {
        ErrorCode = errorCode;
    }
}
