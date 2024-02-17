using Catering.Application.Security.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Security.Handlers;

internal class LoginLdapIdentity : IRequestHandler<LoginLdap, string>
{
    private readonly ITokenAtuhenticator<Identity> _ldapAuthenticator;

    public LoginLdapIdentity(ITokenAtuhenticator<Identity> ldapAuthenticator)
    {
        _ldapAuthenticator = ldapAuthenticator;
    }

    public Task<string> Handle(LoginLdap request, CancellationToken cancellationToken)
    {
        return _ldapAuthenticator.GenerateTokenAsync(request.Login, request.Password);
    }
}
