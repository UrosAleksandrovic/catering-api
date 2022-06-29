using FluentValidation;

namespace Catering.Application.Aggregates.Menus.Dtos.Validators;

internal class UpdateMenuDtoValidator : AbstractValidator<CreateMenuDto>
{
    public UpdateMenuDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 100);
    }
}
