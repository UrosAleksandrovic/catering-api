using Catering.Application.Results;
using Catering.Application.Security.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Security.Handlers;

internal class LoginLdapIdentity : IRequestHandler<LoginLdap, Result<string>>
{
    private readonly ITokenAtuhenticator<Identity> _ldapAuthenticator;

    public LoginLdapIdentity(ITokenAtuhenticator<Identity> ldapAuthenticator)
    {
        _ldapAuthenticator = ldapAuthenticator;
    }

    public async Task<Result<string>> Handle(LoginLdap request, CancellationToken cancellationToken)
        => Result.Success(await _ldapAuthenticator.GenerateTokenAsync(request.Login, request.Password));
}
