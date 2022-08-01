using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Catering.Domain.Exceptions;

namespace Catering.Api.Configuration.ErrorHandling.ErrorHttpResolvers;

public class WrongOrderStatusHttpResolver : ErrorHttpResolver<WrongOrderStatusException>
{
    public override HttpErrorResult Resolve(object error)
        => CheckTypeOfError(error)
            ? new(400, "Action could not be performed on this order because of his status.")
            : HttpErrorResult.DefaultResult;
}
