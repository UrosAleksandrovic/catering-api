using Catering.Api.Configuration.ErrorHandling.Abstractions;

namespace Catering.Api.Configuration.ErrorHandling.ErrorHttpResolvers;

public class KeyNotFoundHttpResolver : ErrorHttpResolver<KeyNotFoundException>
{
    public override HttpErrorResult Resolve(object error)
        => CheckTypeOfError(error)
            ? new(404, "Some resource was not found.")
            : HttpErrorResult.DefaultResult;
}
