using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Catering.Domain.Exceptions;

namespace Catering.Api.Configuration.ErrorHandling.ErrorHttpResolvers
{
    public class InvalidCredentialsHttpResolver : ErrorHttpResolver<InvalidCredentialsException>
    {
        public override HttpErrorResult Resolve(object error)
            => CheckTypeOfError(error)
                ? new(400, "Invalid Credentials.")
                : HttpErrorResult.DefaultResult;
    }
}
