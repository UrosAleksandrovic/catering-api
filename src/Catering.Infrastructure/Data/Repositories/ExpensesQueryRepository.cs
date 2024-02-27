using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Domain.Aggregates.Expense;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class ExpensesQueryRepository : IExpensesQueryRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;
    private readonly IMapper _mapper;

    public ExpensesQueryRepository(
        IDbContextFactory<CateringDbContext> dbContextFactory,
        IMapper mapper)
    {
        _dbContextFactory = dbContextFactory;
        _mapper = mapper;
    }

    public async Task<ExpenseInfoDto> GetByIdAsync(Guid id)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await _mapper.ProjectTo<ExpenseInfoDto>(dbContext
            .Expenses
            .Where(e => e.Id == id)).SingleOrDefaultAsync();
    }

    public async Task<PageBase<ExpenseInfoDto>> GetPageAsync(ExpensesFilter filters)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var filterableOrders = ApplyFilters(filters, dbContext.Expenses);
        if (!filters.OrderBy.HasValue)
        {
            var projectedQuery = _mapper.ProjectTo<ExpenseInfoDto>(filterableOrders);
            return new(await projectedQuery.ToListAsync(), await filterableOrders.CountAsync());
        }

        var projectedOrdered = _mapper.ProjectTo<ExpenseInfoDto>(ApplyOrdering(filters, filterableOrders));
        return new(await projectedOrdered.ToListAsync(), await filterableOrders.CountAsync());
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
