using FluentValidation;

namespace Catering.Application.Aggregates.Items.Dtos.Validators;

internal class UpdateItemDtoValidator : AbstractValidator<UpdateItemDto>
{
    public UpdateItemDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(3, 60);

        RuleFor(dto => dto.Price)
            .GreaterThanOrEqualTo(0);
    }
}
