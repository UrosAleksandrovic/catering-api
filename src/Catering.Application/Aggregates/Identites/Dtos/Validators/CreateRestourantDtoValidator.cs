using FluentValidation;

namespace Catering.Application.Aggregates.Identites.Dtos.Validators;

internal class CreateRestourantDtoValidator : AbstractValidator<CreateRestourantDto>
{
    public CreateRestourantDtoValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        RuleFor(e => e.InitialPassword).NotEmpty();
        RuleFor(e => e.Name).NotEmpty().MinimumLength(2);
    }
}
