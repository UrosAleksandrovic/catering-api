using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CateringIdentityRepository : IdentityRepository<CateringIdentity>, ICateringIdentitiesRepository
{
    public CateringIdentityRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
        : base(dbContextFactory) { }
}
