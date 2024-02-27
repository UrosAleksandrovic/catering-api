using Catering.Application.Results;
using FluentValidation;

namespace Catering.Application.Validation;

internal class ValidationProvider
{
    private readonly IValidatorFactory validatorFactory;

    public ValidationProvider(IValidatorFactory validatorFactory)
    {
        this.validatorFactory = validatorFactory;
    }

    public Result ValidateModel<T>(T model)
        => validatorFactory.GetValidator<T>().Validate(model).ToResult();

    public Result ValidateModel(Type type, object model)
    {
        var context = new ValidationContext<object>(model);
        return validatorFactory.GetValidator(type).Validate(context).ToResult();
    }

    public async Task<Result> ValidateModelAsync<T>(T model)
    {
        var res = await validatorFactory.GetValidator<T>().ValidateAsync(model);
        return res.ToResult();
    }

}
