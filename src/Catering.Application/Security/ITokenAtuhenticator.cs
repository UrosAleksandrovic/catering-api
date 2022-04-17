using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Security
{
    public interface ITokenAtuhenticator<T> : IAuthenticator<T> 
        where T : Identity
    {
        Task<string> TryGenerateTokenAsync(string identity, string password);
    }
}
