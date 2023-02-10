using FluentValidation;

namespace Catering.Application.Aggregates.Identities.Dtos.Validators;

internal class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    public CreateRestaurantDtoValidator()
    {
        RuleFor(e => e.Email).NotEmpty().EmailAddress();
        RuleFor(e => e.InitialPassword).NotEmpty();
        RuleFor(e => e.Name).NotEmpty().MinimumLength(2);
    }
}
