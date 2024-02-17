using System.Security.Claims;
using Catering.Api.Configuration.Authorization;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/cateringIdentities")]
public class CateringIdentityController(ICateringIdentitiesManagementAppService identitiesService) : ControllerBase
{
    private readonly ICateringIdentitiesManagementAppService _identitiesService = identitiesService;

    [HttpPost("invitations")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> SendInvitation([FromBody] CreateIdentityInvitationDto invitationRequest)
    {
        await _identitiesService.SendIdentityInvitationAsync(this.GetUserId(), invitationRequest);

        return NoContent();
    }

    [HttpPost("invitations/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> AcceptInvitation(
        [FromRoute] string id, 
        [FromBody] AcceptInvitationDto acceptInvitationDto)
    {
        if (acceptInvitationDto.HasAccepted)
            await _identitiesService.AcceptInvitationAsync(id, acceptInvitationDto.NewPassword);

        return NoContent();
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<IActionResult> GetIdentityInfo()
    {
        var identityInfoDto = await _identitiesService.GetIdentityInfoAsync(this.GetUserId());

        if (identityInfoDto != null)
            return Ok(identityInfoDto);

        return NotFound();
    }

    [HttpGet("permissions")]
    [Authorize]
    public async Task<IActionResult> GetPermissions()
    {
        return Ok(await _identitiesService.GetIdentityPermissionsAsync(this.GetUserId()));
    }
}
