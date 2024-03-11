using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Expenses.Abstractions;

public interface IExpensesManagementAppService
{
    Task<Result<Guid>> CreateAsync(CreateExpenseDto createExpense);
    Task<Results.Result> UpdateAsync(Guid id, UpdateExpenseDto updateExpense);
}
