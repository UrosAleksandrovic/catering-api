namespace Catering.Api.Configuration.ErrorHandling;

public struct HttpErrorResult
{
    public int HttpStatusCode { get; private set; }
    public string Message { get; private set; }

    public HttpErrorResult(int statusCode, string message)
    {
        HttpStatusCode = statusCode;
        Message = message;
    }

    public static HttpErrorResult DefaultResult => new(500, "Something went wrong");
}
