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
    public async Task<ActionResult<string>> Login([FromBody] UsernameAndPasswordDto customerRequest)
    {
        var request = new LoginCustomer { Login = customerRequest.Username, Password = customerRequest.Password };
        var response = await _publisher.Send(request);

        return Ok(new { token = response });
    }

    [HttpPost("/api/auth/login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> LoginExternalIdentity([FromBody] UsernameAndPasswordDto externalRequest)
    {
        var request = new LoginExternalIdentity { Login = externalRequest.Username, Password = externalRequest.Password };
        var response = await _publisher.Send(request);

        return Ok(new { token = response });
    }
}
