using FluentValidation;

namespace Catering.Application.Aggregates.Identites.Dtos.Validators;

internal class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
