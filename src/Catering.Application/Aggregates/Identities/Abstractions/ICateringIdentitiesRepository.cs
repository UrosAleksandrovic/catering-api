using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICateringIdentitiesRepository : IIdentityRepository<CateringIdentity>
{
    Task<IdentityInvitation> CreateInvitationAsync(IdentityInvitation invitation);
    Task<IdentityInvitation> GetInvitationByIdAsync(string invitationId);
    Task RemoveInvitationAsync(IdentityInvitation invitationId);
}
