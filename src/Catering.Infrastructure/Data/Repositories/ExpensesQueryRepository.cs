using AutoMapper;
using Catering.Application;
using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Expenses.Dtos;
using Catering.Domain.Aggregates.Expense;
using Catering.Infrastructure.EFUtility;
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

        var query = ApplyFilters(filters, dbContext.Expenses);
        query = query.ApplyOrdering(filters.OrderBy, filters.IsOrderByDescending);

        var projectedOrdered = _mapper.ProjectTo<ExpenseInfoDto>(query);
        return new(await projectedOrdered.Paginate(filters).ToListAsync(), await query.CountAsync());
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
}
