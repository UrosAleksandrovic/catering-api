using System.Security.Claims;
using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/cateringIdentities")]
public class CateringIdentityController : ControllerBase
{
    private readonly ICateringIdentitiesManagementAppService _identitiesService;

    public CateringIdentityController(ICateringIdentitiesManagementAppService identitiesService)
    {
        _identitiesService = identitiesService;
    }

    [HttpPost("invite")]
    [AuthorizeClientsAdmins]
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

    [HttpGet("info")]
    [Authorize]
    public async Task<IActionResult> GetIdentityInfo()
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var identityInfoDto = await _identitiesService.GetIdentityInfoAsync(requesterId);

        if (identityInfoDto != null)
            return Ok(identityInfoDto);

        return NotFound();
    }

    [HttpGet("permissions")]
    [Authorize]
    public async Task<IActionResult> GetPermissions()
    {
        var requesterId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return Ok(await _identitiesService.GetIdentityPermissionsAsync(requesterId));
    }
}
