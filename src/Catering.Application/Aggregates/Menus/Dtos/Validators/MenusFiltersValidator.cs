using FluentValidation;

namespace Catering.Application.Aggregates.Menus.Dtos.Validators;

internal class MenusFiltersValidator : AbstractValidator<MenusFilter>
{
    public MenusFiltersValidator()
    {
        RuleFor(x => x).SetValidator(new FilterBaseValidator());
    }
}
