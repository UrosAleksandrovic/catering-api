using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CateringIdentityRepository : IdentityRepository<CateringIdentity>, ICateringIdentitiesRepository
{
    public CateringIdentityRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
        : base(dbContextFactory) { }

    public async Task<IdentityInvitation> CreateInvitationAsync(IdentityInvitation invitation)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        await dbContext.Set<IdentityInvitation>().AddAsync(invitation);
        await dbContext.SaveChangesAsync();

        return invitation;
    }

    public async Task<IdentityInvitation> GetInvitationByIdAsync(string invitationId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var result = await dbContext.Set<IdentityInvitation>().FindAsync(invitationId);

        return result;
    }

    public async Task RemoveInvitationAsync(IdentityInvitation invitation)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        dbContext.Set<IdentityInvitation>().Remove(invitation);
        await dbContext.SaveChangesAsync();
    }
}
