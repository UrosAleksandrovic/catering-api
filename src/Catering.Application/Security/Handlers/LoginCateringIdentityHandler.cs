using Catering.Application.Security.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Security.Handlers;

internal class LoginCateringIdentityHandler : IRequestHandler<LoginCateringIdentity, string>
{
    private readonly ITokenAtuhenticator<CateringIdentity> _cateringIdentityAuthenticator;

    public LoginCateringIdentityHandler(ITokenAtuhenticator<CateringIdentity> cateringIdentityAuthenticator)
    {
        _cateringIdentityAuthenticator = cateringIdentityAuthenticator;
    }

    public Task<string> Handle(LoginCateringIdentity request, CancellationToken cancellationToken)
    {
        return _cateringIdentityAuthenticator.GenerateTokenAsync(request.Login, request.Password);
    }
}
