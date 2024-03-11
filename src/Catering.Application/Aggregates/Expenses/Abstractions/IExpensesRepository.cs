using Catering.Domain.Aggregates.Expense;

namespace Catering.Application.Aggregates.Expenses.Abstractions;

public interface IExpensesRepository : IBaseCrudRepository<Expense>
{
}
