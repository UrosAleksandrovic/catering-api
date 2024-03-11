using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICateringIdentitiesManagementAppService
{
    Task<Result> SendIdentityInvitationAsync(string creatorId, CreateIdentityInvitationDto createRequest);
    Task<Result> AcceptInvitationAsync(string invitationId, string newPassword);
}
