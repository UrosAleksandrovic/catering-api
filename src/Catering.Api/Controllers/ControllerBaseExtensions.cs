using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

public static class ControllerBaseExtensions
{
    public static string GetUserId(this ControllerBase controller)
        => controller.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
}
