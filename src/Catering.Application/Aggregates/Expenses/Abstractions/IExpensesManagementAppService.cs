using Catering.Application.Aggregates.Expenses.Dtos;

namespace Catering.Application.Aggregates.Expenses.Abstractions
{
    public interface IExpensesManagementAppService
    {
        Task<Guid> CreateAsync(CreateExpenseDto createExpense);
        Task<ExpenseInfoDto> GetByIdAsync(Guid id, string requestorId);
        Task<FilterResult<ExpenseInfoDto>> GetFilteredAsync(ExpensesFilter filters);
        Task UpdateAsync(Guid id, UpdateExpenseDto updateExpense);
    }
}
