using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface IIdentityRepository<T> : IBaseCrudRepository<T> where T : Identity
{
    Task<T> GetByEmailAsync(string email);
}
