using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface IIdentityQueryRepository<TIdentity> where TIdentity : Identity
{
    Task<PageBase<TDto>> GetPageAsync<TDto>(IdentityFilter filter);
    Task<TDto> GetByIdAsync<TDto>(string id);
}
