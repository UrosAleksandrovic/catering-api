using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.Application.Validation;

internal class ValidatorFactory
{
    private readonly IServiceProvider serviceProvider;

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IValidator<T> GetValidator<T>()
        => serviceProvider.GetRequiredService<IValidator<T>>();

    public IValidator GetValidator(Type type)
    {
        var genericType = typeof(IValidator<>).MakeGenericType(type);
        return (IValidator)serviceProvider.GetRequiredService(genericType);
    }
}
