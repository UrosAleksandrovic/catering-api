using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Catering.Domain.Exceptions;

namespace Catering.Api.Configuration.ErrorHandling.Handlers;

public class IndentityRestrictionHttpResolver : ErrorHttpResolver<IdentityRestrictionException>
{
    public override HttpErrorResult Resolve(object error)
        => CheckTypeOfError(error)
            ? new(403, "Logged user not allowed to perform an action")
            : HttpErrorResult.DefaultResult;
}
