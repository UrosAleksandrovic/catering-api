using FluentValidation;

namespace Catering.Application.Aggregates.Expenses.Dtos.Validators;

internal class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.MenuId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.DeliveredOn).NotEmpty();
        RuleFor(x => x.Note).MaximumLength(3000);
    }
}
