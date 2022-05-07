using FluentValidation;

namespace Catering.Application.Aggregates.Items.Dtos.Validators;

internal class CreateItemDtoValidator : AbstractValidator<CreateItemDto>
{
    public CreateItemDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 60);

        RuleFor(dto => dto.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(dto => dto.MenuId)
            .NotEmpty();
    }
}
