using FluentValidation;

namespace Catering.Application.Validation;

public interface IValidatorFactory
{
    IValidator<T> GetValidator<T>();
    IValidator GetValidator(Type type);
}
