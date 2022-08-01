using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Catering.Domain.Exceptions;

namespace Catering.Api.Configuration.ErrorHandling.ErrorHttpResolvers;

public class ItemAlreadyInCartHttpResolver : ErrorHttpResolver<ItemAlreadyInCartException>
{
    public override HttpErrorResult Resolve(object error)
        => CheckTypeOfError(error)
            ? new(400, "Item is already in cart.")
            : HttpErrorResult.DefaultResult;
}
