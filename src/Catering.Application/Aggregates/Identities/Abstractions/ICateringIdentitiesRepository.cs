using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICateringIdentitiesRepository : IIdentityRepository<CateringIdentity>
{ }
