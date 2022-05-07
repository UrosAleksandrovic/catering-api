using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Security;

public interface ITokenAtuhenticator<T> : IAuthenticator<T> 
    where T : Identity
{
    Task<string> GenerateTokenAsync(string identity, string password);
}
