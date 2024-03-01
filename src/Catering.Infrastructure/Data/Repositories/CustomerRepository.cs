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

    public async Task CreateAsync(Customer customer, Identity identity)
    {
        _dbContext.Identities.Add(identity);
        _dbContext.Customers.Add(customer);

        await _dbContext.SaveChangesAsync();
    }
}
