using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICateringIdentitiesManagementAppService
{
    Task SendIdentityInvitationAsync(string creatorId, CreateIdentityInvitationDto createRequest);
    Task AcceptInvitationAsync(string invitationId, string newPassword);
    Task<IdentityInfoDto> GetIdentityInfoAsync(string identityId);
    Task<IdentityPermissionsDto> GetIdentityPermissionsAsync(string identityId);
}
