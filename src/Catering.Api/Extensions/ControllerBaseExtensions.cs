using System.Security.Claims;
using Catering.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Extensions;

public static class ControllerBaseExtensions
{
    public static IActionResult MapErrorResultToActionResult(this Result result)
    {
        return result?.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(result),
            ErrorType.ValidationError => new BadRequestObjectResult(result),
            ErrorType.Unknown => new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
            _ => GetDefaultResponse(),
        };
    }

    public static IActionResult FromResult<T>(this ControllerBase controller, Result<T> result)
    {
        if (result == null)
        {
            return GetDefaultResponse();
        }

        if (result.ErrorCodes.Any() || !result.IsSuccess)
        {
            return MapErrorResultToActionResult(result);
        }

        if (result.Value is null)
        {
            return new OkResult();
        }

        return new OkObjectResult(result.Value);
    }

    public static IActionResult FromResult(this ControllerBase controller, Result result)
    {
        if (result == null)
        {
            return GetDefaultResponse();
        }

        if (result.ErrorCodes.Any() || !result.IsSuccess)
        {
            return MapErrorResultToActionResult(result);
        }

        return new NoContentResult();
    }

    public static IActionResult CreatedAtRouteFromResult<TId>(
        this ControllerBase controller,
        Result<TId> result,
        string route)
    {
        if (result == null)
        {
            return GetDefaultResponse();
        }

        if (result.ErrorCodes.Any() || !result.IsSuccess)
        {
            return MapErrorResultToActionResult(result);
        }

        var model = new { Id = result.Value };

        return new CreatedAtRouteResult(route, model, model);
    }

    private static ObjectResult GetDefaultResponse() => new ObjectResult(Result.Error(ErrorType.Unknown))
    {
        StatusCode = StatusCodes.Status500InternalServerError
    };

    public static string GetUserId(this ControllerBase controller)
        => controller.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
}
