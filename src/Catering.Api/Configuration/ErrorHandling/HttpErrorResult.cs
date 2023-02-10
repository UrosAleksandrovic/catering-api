namespace Catering.Api.Configuration.ErrorHandling;

public struct HttpErrorResult
{
    public int HttpStatusCode { get; }
    public string Message { get; }

    public HttpErrorResult(int statusCode, string message)
    {
        HttpStatusCode = statusCode;
        Message = message;
    }

    public static HttpErrorResult DefaultResult => new(500, "Something went wrong");
}
