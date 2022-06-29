using FluentValidation;

namespace Catering.Application.Aggregates.Menus.Dtos.Validators;

internal class CreateMenuDtoValidator : AbstractValidator<CreateMenuDto>
{
    public CreateMenuDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(dto => dto.ContactIdentityId)
            .NotEmpty();
    }
}
