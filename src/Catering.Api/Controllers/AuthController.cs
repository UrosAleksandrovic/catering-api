using Catering.Application.Security.Dtos;
using Catering.Application.Security.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

public class AuthController : ControllerBase
{
    private readonly IMediator _publisher;

    public AuthController(IMediator publisher)
    {
        _publisher = publisher;
    }

    [HttpPost("/api/auth/ldap/login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginLdap([FromBody] UsernameAndPasswordDto request)
    {
        var identityRequest = new LoginLdap { Login = request.Username, Password = request.Password };
        var response = await _publisher.Send(identityRequest);

        return Ok(new { token = response });
    }

    [HttpPost("/api/auth/login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginCateringIdentity([FromBody] UsernameAndPasswordDto request)
    {
        var identityRequest = new LoginCateringIdentity { Login = request.Username, Password = request.Password };
        var response = await _publisher.Send(identityRequest);

        return Ok(new { token = response });
    }
}
