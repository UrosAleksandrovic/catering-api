using Catering.Application.Security.Requests;
using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Security.Handlers;

internal class LoginExternalIndentityHandler : IRequestHandler<LoginExternalIdentity, string>
{
    private readonly ITokenAtuhenticator<ExternalIdentity> _externalIdentityAuthenticator;

    public LoginExternalIndentityHandler(ITokenAtuhenticator<ExternalIdentity> externalIdentityAuthenticator)
    {
        _externalIdentityAuthenticator = externalIdentityAuthenticator;
    }

    public Task<string> Handle(LoginExternalIdentity request, CancellationToken cancellationToken)
    {
        return _externalIdentityAuthenticator.GenerateTokenAsync(request.Login, request.Password);
    }
}
