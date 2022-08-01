namespace Catering.Api.Configuration.ErrorHandling.Abstractions;

public abstract class ErrorHttpResolver<TException> : IErrorHttpResolver
    where TException : Exception
{
    public abstract HttpErrorResult Resolve(object error);

    protected bool CheckTypeOfError(object error)
        => error is TException;
}
