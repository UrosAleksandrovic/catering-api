using Catering.Application.Aggregates.Menus;
using Catering.Domain.Entities.ExpenseAggregate;

namespace Catering.Application.Aggregates.Expenses.Abstractions
{
    public interface IExpensesRepository : IBaseCrudRepository<Expense>
    {
        Task<(List<Expense> Expenses, int TotalCount)> GetFilteredAsync(ExpensesFilter expensesFilter);
    }
}
