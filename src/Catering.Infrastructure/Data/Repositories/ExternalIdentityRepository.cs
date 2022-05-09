using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ExternalIdentityRepository : IdentityRepository<ExternalIdentity>, IExternalIdentityRepository
{
    public ExternalIdentityRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
        : base(dbContextFactory) { }
}
