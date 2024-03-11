using Catering.Application.Results;
using Catering.Application.Security.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Security.Handlers;

internal class LoginCateringIdentityHandler : IRequestHandler<LoginCateringIdentity, Result<string>>
{
    private readonly ITokenAtuhenticator<CateringIdentity> _cateringIdentityAuthenticator;

    public LoginCateringIdentityHandler(ITokenAtuhenticator<CateringIdentity> cateringIdentityAuthenticator)
    {
        _cateringIdentityAuthenticator = cateringIdentityAuthenticator;
    }

    public async Task<Result<string>> Handle(LoginCateringIdentity request, CancellationToken cancellationToken)
        => Result.Success(await _cateringIdentityAuthenticator.GenerateTokenAsync(request.Login, request.Password));
}
