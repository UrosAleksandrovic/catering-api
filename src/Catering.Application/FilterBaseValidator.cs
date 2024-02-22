using FluentValidation;

namespace Catering.Application;

internal class FilterBaseValidator : AbstractValidator<FilterBase>
{
    public FilterBaseValidator()
    {
        RuleFor(x => x.PageIndex).Must(x => x >= 1);
        RuleFor(x => x.PageSize).Must(x => x >= 1);
    }
}
