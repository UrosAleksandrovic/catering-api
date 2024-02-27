using Catering.Application.Results;
using FluentValidation.Results;

namespace Catering.Application.Validation;

internal static class ValidatorExtensions
{
    public static Result ToResult(this ValidationResult validationResult)
    {
        if (!validationResult.IsValid)
        {
            return Result.ValidationError(validationResult.Errors.Select(x => x.ErrorCode).ToArray());
        }

        return Result.Success();
    }

}
