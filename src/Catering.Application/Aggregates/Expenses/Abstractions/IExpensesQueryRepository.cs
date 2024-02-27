using Catering.Application.Aggregates.Expenses.Dtos;

namespace Catering.Application.Aggregates.Expenses.Abstractions;

public interface IExpensesQueryRepository
{
    Task<ExpenseInfoDto> GetByIdAsync(Guid id);
    Task<PageBase<ExpenseInfoDto>> GetPageAsync(ExpensesFilter expensesFilter);
}
