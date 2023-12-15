using System.Security.Authentication;
using Catering.Api.Configuration.ErrorHandling.Abstractions;

namespace Catering.Api.Configuration.ErrorHandling.ErrorHttpResolvers
{
    public class AuthenticationHttpResolver : ErrorHttpResolver<AuthenticationException>
    {
        public override HttpErrorResult Resolve(object error)
            => CheckTypeOfError(error)
                ? new(400, "Wrong email or password")
                : HttpErrorResult.DefaultResult;
    }
}
