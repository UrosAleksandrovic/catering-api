using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Catering.Domain.ErrorCodes;
using Catering.Domain.Exceptions;

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
        catch(CateringException e)
        {
            var hasParameters = e.Data != default && e.Data.Count > 0;
            if (hasParameters)
            {
                var parameters = new object[e.Data.Count];
                e.Data.Values.CopyTo(parameters, 0);
                logger.LogError(e, e.Message, parameters);
            }
            else
                logger.LogError(e, e.Message);


            var resolvedError = errorPublisher.Publish(e) ?? HttpErrorResult.DefaultResult;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = resolvedError.HttpStatusCode;
            await context.Response.WriteAsJsonAsync(new { e.ErrorCode });
        }
        catch(Exception e)
        {
            logger.LogError(e, e.Message);

            var resolvedError = errorPublisher.Publish(e) ?? HttpErrorResult.DefaultResult;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = resolvedError.HttpStatusCode;
            await context.Response.WriteAsJsonAsync(new { ErrorCode = ErrorCodes.UNKNOWN_ERROR });
        }
    }
}
