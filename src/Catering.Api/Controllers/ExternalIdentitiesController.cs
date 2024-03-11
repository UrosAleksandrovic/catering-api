using Catering.Api.Configuration.Authorization;
using Catering.Api.Extensions;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/identities/externals")]
public class ExternalIdentitiesController(ICateringIdentitiesManagementAppService identitiesService) : ControllerBase
{
    private readonly ICateringIdentitiesManagementAppService _identitiesService = identitiesService;

    [HttpPost("invitations")]
    [AuthorizeClientsAdmins]
    public async Task<IActionResult> SendInvitation([FromBody] CreateIdentityInvitationDto invitationRequest)
        => this.FromResult(await _identitiesService.SendIdentityInvitationAsync(this.GetUserId(), invitationRequest));

    [HttpPost("invitations/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> AcceptInvitation(
        [FromRoute] string id,
        [FromBody] AcceptInvitationDto acceptInvitationDto)
    {
        Result result = Result.UnknownError();
        if (acceptInvitationDto.HasAccepted)
            result = await _identitiesService.AcceptInvitationAsync(id, acceptInvitationDto.NewPassword);

        return this.FromResult(result);
    }
}
