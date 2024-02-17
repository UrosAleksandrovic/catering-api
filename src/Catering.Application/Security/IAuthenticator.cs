using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Security;

public interface IAuthenticator<T> where T : Identity
{
    Task<T> AuthenticateAsync(string identity, string password);
}
