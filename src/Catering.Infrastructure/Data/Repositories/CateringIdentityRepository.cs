using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Aggregates.Identity;

namespace Catering.Infrastructure.Data.Repositories;

internal class CateringIdentityRepository : IdentityRepository<CateringIdentity>, ICateringIdentitiesRepository
{
    public CateringIdentityRepository(CateringDbContext dbContext)
        : base(dbContext) { }

    public async Task<IdentityInvitation> CreateInvitationAsync(IdentityInvitation invitation)
    {
        await _dbContext.Set<IdentityInvitation>().AddAsync(invitation);
        await _dbContext.SaveChangesAsync();

        return invitation;
    }

    public async Task<IdentityInvitation> GetInvitationByIdAsync(string invitationId)
        => await _dbContext.Set<IdentityInvitation>().FindAsync(invitationId);

    public async Task RemoveInvitationAsync(IdentityInvitation invitation)
    {
        _dbContext.Set<IdentityInvitation>().Remove(invitation);
        await _dbContext.SaveChangesAsync();
    }
}
