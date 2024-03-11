using Catering.Api.Extensions;
using Catering.Application.Security.Dtos;
using Catering.Application.Security.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

public class AuthController(IMediator publisher) : ControllerBase
{
    private readonly IMediator _publisher = publisher;

    [HttpPost("/api/auth/ldap/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginLdap([FromBody] UsernameAndPasswordDto request)
    {
        var response = await _publisher.Send(new LoginLdap(request.Username, request.Password));

        return this.FromResult(response);
    }

    [HttpPost("/api/auth/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginCateringIdentity([FromBody] UsernameAndPasswordDto request)
    {
        var response = await _publisher.Send(new LoginCateringIdentity(request.Username, request.Password));

        return this.FromResult(response);
    }
}
