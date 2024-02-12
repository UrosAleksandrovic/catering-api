using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Domain.Entities.ExpenseAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ExpensesRepository : BaseCrudRepository<Expense, CateringDbContext>, IExpensesRepository
{
    public ExpensesRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
        : base(dbContextFactory)
    { }

    public async Task<(List<Expense> Expenses, int TotalCount)> GetFilteredAsync(ExpensesFilter filters)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var filterableOrders = ApplyFilters(filters, dbContext.Set<Expense>());
        if (!filters.OrderBy.HasValue)
            return (await filterableOrders.ToListAsync(), await filterableOrders.CountAsync());

        filterableOrders = ApplyOrdering(filters, filterableOrders);

        return (await filterableOrders.ToListAsync(), await filterableOrders.CountAsync());
    }

    private IQueryable<Expense> ApplyFilters(ExpensesFilter filters, IQueryable<Expense> expenses)
    {
        expenses.AsNoTracking();

        if (filters.CustomerIds != null && filters.CustomerIds.Any())
            expenses = expenses.Where(e => filters.CustomerIds.Contains(e.CustomerId));

        if (filters.DeliveredFrom.HasValue)
            expenses = expenses.Where(e => e.DeliveredOn >= filters.DeliveredFrom.Value);

        if (filters.DeliveredTo.HasValue)
            expenses = expenses.Where(e => e.DeliveredOn <= filters.DeliveredTo.Value);

        return expenses
            .Skip((filters.PageIndex - 1) * filters.PageSize)
            .Take(filters.PageSize);
    }

    private IOrderedQueryable<Expense> ApplyOrdering(ExpensesFilter filters, IQueryable<Expense> query)
    {
        return filters switch
        {
            { OrderBy: ExpensesOrderBy.Price, IsOrderByDescending: false } => query.OrderBy(i => i.Price),
            { OrderBy: ExpensesOrderBy.Price, IsOrderByDescending: true } => query.OrderByDescending(i => i.Price),

            { OrderBy: ExpensesOrderBy.DeliveredOn, IsOrderByDescending: false } => query.OrderBy(i => i.DeliveredOn),
            { OrderBy: ExpensesOrderBy.DeliveredOn, IsOrderByDescending: true } => query.OrderByDescending(i => i.DeliveredOn),

            _ => query.OrderByDescending(i => i.DeliveredOn),
        };
    }
}
