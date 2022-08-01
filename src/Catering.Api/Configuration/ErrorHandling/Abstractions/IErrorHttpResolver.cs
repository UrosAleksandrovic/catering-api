namespace Catering.Api.Configuration.ErrorHandling.Abstractions;

public interface IErrorHttpResolver
{
    HttpErrorResult Resolve(object error);
}
