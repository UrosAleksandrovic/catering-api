using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Domain.Aggregates.Expense;

namespace Catering.Infrastructure.Data.Repositories;

internal class ExpensesRepository : BaseCrudRepository<Expense, CateringDbContext>, IExpensesRepository
{
    public ExpensesRepository(CateringDbContext dbContext)
        : base(dbContext)
    { }
}
