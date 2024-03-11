using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface IIdentityRepository<T> : IBaseCrudRepository<T> where T : Identity
{
    Task CompleteInvitationAsync(Identity identity, Customer customer, IdentityInvitation invitation);
    Task<T> GetByEmailAsync(string email);
}
