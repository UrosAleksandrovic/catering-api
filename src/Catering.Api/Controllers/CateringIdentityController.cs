using System.Security.Claims;
using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/cateringIdentities")]
[AuthorizeClientsAdmins]
public class CateringIdentityController : ControllerBase
{
    private readonly ICateringIdentitiesManagementAppService _identitiesService;

    public CateringIdentityController(ICateringIdentitiesManagementAppService identitiesService)
    {
        _identitiesService = identitiesService;
    }

    [HttpPost("invite")]
    public async Task<IActionResult> SendInvitation([FromBody] CreateIdentityInvitationDto invitationRequest)
    {
        var requestorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _identitiesService.SendIdentityInvitationAsync(requestorId, invitationRequest);

        return NoContent();
    }

    [HttpPost("invitation/{id}/accept")]
    [AllowAnonymous]
    public async Task<IActionResult> AcceptInvitation([FromRoute] string id, [FromBody] AcceptInvitationDto acceptInvitationDto)
    {
        await _identitiesService.AcceptInvitationAsync(id, acceptInvitationDto.NewPassword);

        return NoContent();
    }
}
