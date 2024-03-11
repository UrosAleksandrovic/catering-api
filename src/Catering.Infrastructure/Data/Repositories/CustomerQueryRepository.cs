using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CustomerQueryRepository : ICustomerQueryRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;

    public CustomerQueryRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<CustomerBudgetInfoDto> GetCustomerBudgetAsync(string customerId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Customers
            .AsNoTracking()
            .Where(c => c.IdentityId == customerId)
            .Select(c => new CustomerBudgetInfoDto
            {
                Budget = c.Budget.Balance,
                ReservedBalance = c.Budget.ReservedAssets
            })
            .SingleOrDefaultAsync();
    }

    public async Task<CustomerInfoDto> GetCustomerInfoAsync(string customerId)
    {
        using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        return await dbContext.Customers
            .AsNoTracking()
            .Where(c => c.IdentityId == customerId)
            .Select(c => new CustomerInfoDto
            {
                IdentityInfo = new IdentityInfoDto
                {
                    Id = c.IdentityId,
                    Email = c.Identity.Email,
                    FirstName = c.Identity.FullName.FirstName,
                    LastName = c.Identity.FullName.LastName,
                    Role = c.Identity.Role.ToString(),
                },
                CustomerBudgetInfo = new CustomerBudgetInfoDto
                {
                    Budget = c.Budget.Balance,
                    ReservedBalance = c.Budget.ReservedAssets
                }
            })
            .SingleOrDefaultAsync();
    }
}
