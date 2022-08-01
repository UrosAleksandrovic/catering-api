using FluentValidation;

namespace Catering.Application.Security.Dtos.Validators;

internal class UsernameAndPasswordValidator : AbstractValidator<UsernameAndPasswordDto>
{
    public UsernameAndPasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(2);
    }
}
