using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Catering.Domain.Exceptions;

namespace Catering.Api.Configuration.ErrorHandling.ErrorHttpResolvers;

public class ItemMenuNotValidHttpResolver : ErrorHttpResolver<ItemMenuNotValidException>
{
    public override HttpErrorResult Resolve(object error)
        => CheckTypeOfError(error)
            ? new(400, "Cart items have to be from the same menu.")
            : HttpErrorResult.DefaultResult;
}
