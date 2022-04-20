using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface IIdentityRepository<T> where T : Identity
{
    Task<T> GetByEmailAsync(string email);
}
