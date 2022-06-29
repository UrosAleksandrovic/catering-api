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
    public async Task<ActionResult<string>> Login(
        [FromBody] string username, 
        [FromBody] string password)
    {
        var response = await _publisher.Send(new LoginCustomer { Login = username, Password = password });

        return Ok(new { token = response });
    }

    [HttpPost("/api/auth/login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginExternalIdentity(
        [FromBody] string username,
        [FromBody] string password)
    {
        var response = await _publisher.Send(new LoginExternalIdentity { Login = username, Password = password });

        return Ok(new { token = response });
    }
}
