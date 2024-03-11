using FluentValidation;

namespace Catering.Application.Aggregates.Carts.Dtos.Validators;

internal class AddItemToCartValidator : AbstractValidator<AddItemToCartDto>
{
    public AddItemToCartValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.MenuId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Note).MaximumLength(500);
    }
}
