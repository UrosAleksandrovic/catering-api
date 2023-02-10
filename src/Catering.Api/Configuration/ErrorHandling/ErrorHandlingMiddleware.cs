using Catering.Api.Configuration.ErrorHandling.Abstractions;

namespace Catering.Api.Configuration.ErrorHandling;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogger<ErrorHandlingMiddleware> logger,
        IErrorPublisher errorPublisher)
    {
        try
        {
            await _next(context);
        }
        catch(Exception e)
        {
            logger.LogError(e, e.Message);

            var resolvedError = errorPublisher.Publish(e) ?? HttpErrorResult.DefaultResult;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = resolvedError.HttpStatusCode;
            await context.Response.WriteAsJsonAsync(new { message = resolvedError.Message });
        }
    }
}
