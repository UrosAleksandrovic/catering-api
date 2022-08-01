namespace Catering.Api.Configuration.ErrorHandling.Abstractions;

public interface IErrorPublisher
{
    HttpErrorResult? Publish(Exception e);
}
