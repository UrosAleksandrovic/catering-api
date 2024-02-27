using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Filtering;
using MediatR;

namespace Catering.Application.Aggregates.Expenses.Queries;

public record class GetFilteredExpensesQuery(ExpensesFilter Filters) : IRequest<FilterResult<ExpenseInfoDto>>;

public class GetFilteredExpensesQueryHandler(IExpensesQueryRepository queryRepository) 
    : IRequestHandler<GetFilteredExpensesQuery, FilterResult<ExpenseInfoDto>>
{
    private readonly IExpensesQueryRepository _queryRepository = queryRepository;

    public async Task<FilterResult<ExpenseInfoDto>> Handle(
        GetFilteredExpensesQuery request,
        CancellationToken cancellationToken)
    {
        var result = FilterResult<ItemInfoDto>.Empty<ExpenseInfoDto>(
            request.Filters.PageSize,
            request.Filters.PageIndex);

        var pageBase = await _queryRepository.GetPageAsync(request.Filters);
        result.TotalNumberOfElements = pageBase.TotalCount;
        result.Value = pageBase.Data;

        return result;
    }
}
