using Catering.Application.Aggregates.Identities;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Domain.Aggregates.Identity;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CustomerRepository : BaseCrudRepository<Customer, CateringDbContext>, ICustomerRepository
{
    public CustomerRepository(CateringDbContext dbContext) 
        : base(dbContext) { }

    public async Task<Customer> GetByIdentityEmailAsync(string email)
        => await _dbContext.Set<Customer>()
            .AsNoTracking()
            .Include(c => c.Identity)
            .Where(c => c.Identity.Email == email)
            .FirstOrDefaultAsync();

    public async Task<(IEnumerable<Customer>, int)> GetFilteredInternalCustomersAsync(CustomersFilter filter)
    {
        var queryable = _dbContext
            .Customers
            .AsNoTracking()
            .Include(c => c.Identity)
            .GroupJoin(
                _dbContext.CateringIdentities,
                customer => customer.IdentityId,
                cateringIdentity => cateringIdentity.Id,
                (customer, cateringIdentity) => new { Customer = customer, CateringIdentity = cateringIdentity })
            .SelectMany(
                r => r.CateringIdentity.DefaultIfEmpty(),
                (r, cateringIdentity) => new { r.Customer, CateringIdentity = cateringIdentity })
            .Where(c => c.CateringIdentity == null)
            .Select(c => c.Customer);

        var resultQuery = ApplyFilters(queryable, filter);

        return (await resultQuery.ToListAsync(),
                await resultQuery.CountAsync());
    }

    public async Task<(IEnumerable<Customer>, int)> GetFilteredExternalCustomersAsync(CustomersFilter filter)
    {
        var queryable = _dbContext
            .Customers
            .AsNoTracking()
            .Join(_dbContext.CateringIdentities,
                customer => customer.IdentityId,
                cateringIdentity => cateringIdentity.Id,
                (customer, cateringIdentity) => customer)
            .Include(c => c.Identity);

        var resultQuery = ApplyFilters(queryable, filter);

        return (await queryable.ToListAsync(),
                await queryable.CountAsync());
    }

    public async Task<Customer> GetFullByIdAsync(string id)
    {
        return await _dbContext
            .Set<Customer>()
            .AsNoTracking()
            .Include(c => c.Identity)
            .Where(c => c.IdentityId == id)
            .FirstOrDefaultAsync();
    }

    public async Task ResetBudgetAsync(decimal newBudget)
    {
        await _dbContext.Customers.ExecuteUpdateAsync(setter => setter
            .SetProperty(c => c.Budget.Balance, newBudget)
            .SetProperty(c => c.Budget.ReservedAssets, 0));

        await _dbContext.SaveChangesAsync();
    }

    private IQueryable<Customer> ApplyFilters(IQueryable<Customer> customers, CustomersFilter filter)
    {
        var finalFilter = customers;

        if (filter.IsAdministrator != default)
            finalFilter = finalFilter.Where(c => c.Identity.Role.IsAdministrator());

        if (filter.FirstName != default)
            finalFilter = finalFilter.Where(c => EF.Functions.Like(c.Identity.FullName.FirstName, $"%{filter.FirstName}%"));

        if (filter.LastName != default)
            finalFilter = finalFilter.Where(c => EF.Functions.Like(c.Identity.FullName.FirstName, $"%{filter.LastName}%"));

        if (filter.Email != default)
            finalFilter = finalFilter.Where(c => EF.Functions.Like(c.Identity.Email, $"%{filter.Email}%"));

        if (filter.MaxBalance != default)
            finalFilter = finalFilter.Where(c => c.Budget.Balance < filter.MaxBalance);

        if (filter.MinBalance != default)
            finalFilter = finalFilter.Where(c => c.Budget.Balance < filter.MinBalance);

        if (filter.Role != default)
            finalFilter = finalFilter.Where(c => c.Identity.Role.HasFlag(filter.Role.Value));

        return finalFilter.Skip((filter.PageIndex - 1) * filter.PageSize)
                          .Take(filter.PageSize);
    }
}
