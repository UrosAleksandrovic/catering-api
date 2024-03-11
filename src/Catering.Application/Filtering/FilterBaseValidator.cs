using FluentValidation;

namespace Catering.Application.Filtering;

internal class FilterBaseValidator : AbstractValidator<FilterBase>
{
    public FilterBaseValidator()
    {
        RuleFor(x => x.PageIndex).Must(x => x >= 1);
        RuleFor(x => x.PageSize).Must(x => x >= 1);
    }
}
