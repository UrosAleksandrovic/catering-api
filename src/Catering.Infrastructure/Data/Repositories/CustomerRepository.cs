using Catering.Application.Aggregates.Identites;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CustomerRepository : IdentityRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IDbContextFactory<CateringDbContext> dbContextFactory) 
        : base(dbContextFactory) { }

    public async Task<(IEnumerable<Customer>, int)> GetFilteredAsync(CustomersFilter filter)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var resultQuery = ApplyFilters(dbContext.Customers.AsNoTracking(), filter);

        return (await resultQuery.ToListAsync(),
                await resultQuery.CountAsync());
    }

    private IQueryable<Customer> ApplyFilters(IQueryable<Customer> customers, CustomersFilter filter)
    {
        var finalFilter = customers;

        if (filter.IsAdministrator != default)
            finalFilter = finalFilter.Where(c => c.IsAdministrator);

        if (filter.FirstName != default)
            finalFilter = finalFilter.Where(c => EF.Functions.Like(c.FullName.FirstName, $"%{filter.FirstName}%"));

        if (filter.LastName != default)
            finalFilter = finalFilter.Where(c => EF.Functions.Like(c.FullName.FirstName, $"%{filter.LastName}%"));

        if (filter.Email != default)
            finalFilter = finalFilter.Where(c => EF.Functions.Like(c.Email, $"%{filter.Email}%"));

        if (filter.MaxBalance != default)
            finalFilter = finalFilter.Where(c => c.Budget.Balance < filter.MaxBalance);

        if (filter.MinBalance != default)
            finalFilter = finalFilter.Where(c => c.Budget.Balance < filter.MinBalance);

        if (filter.Role != default)
            finalFilter = finalFilter.Where(c => c.Roles.Contains(filter.Role));

        return finalFilter.Skip((filter.PageIndex - 1) * filter.PageSize)
                          .Take(filter.PageSize);
    }
}
