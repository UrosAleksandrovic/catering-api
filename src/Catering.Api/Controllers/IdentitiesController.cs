using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Identities;
using Catering.Application.Aggregates.Identities.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/identities")]
public class IdentitiesController(IMediator publisher) : ControllerBase
{
    private readonly IMediator _publisher = publisher;

    [HttpGet("profile/permissions")]
    [Authorize]
    public async Task<IActionResult> GetPermissions()
        => this.FromResult(await _publisher.Send(new GetIdentityPermissionsQuery(this.GetUserId())));

    [HttpGet]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> GetFilteredAsync([FromQuery] IdentityFilter filters)
        => this.FromResult(await _publisher.Send(new GetFilteredIdentitiesQuery(filters)));
}
