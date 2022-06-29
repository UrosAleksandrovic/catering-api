using FluentValidation;

namespace Catering.Application.Aggregates.Orders.Dtos.Validators;

internal class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(dto => dto.ExpectedTimeOfDelivery)
            .Must(v => v != default);
    }
}
