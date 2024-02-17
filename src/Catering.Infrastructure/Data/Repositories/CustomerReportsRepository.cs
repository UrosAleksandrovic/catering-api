using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Domain.Aggregates.Order;
using Microsoft.EntityFrameworkCore;

namespace Catering.Infrastructure.Data.Repositories;

internal class CustomerReportsRepository : ICustomerReportsRepository
{
    private readonly IDbContextFactory<CateringDbContext> _dbContextFactory;

    public CustomerReportsRepository(IDbContextFactory<CateringDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<CustomerMonthlySpendingDto>> GetMonthlySendingReportAsync(int month, int year)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var orderData = dbContext.Orders.AsNoTracking()
            .Where(o => o.CreatedOn.Month == month)
            .Where(o => o.CreatedOn.Year == year)
            .Where(o => o.Status == OrderStatus.Confirmed)
            .GroupBy(o => o.CustomerId)
            .Select(o => new
            {
                CustomerId = o.Key,
                TotalSpent = o.Sum(x => x.Items.Sum(y => y.Quantity * y.PriceSnapshot))
            });

        var expensesData = dbContext.Expenses.AsNoTracking()
            .Where(e => e.DeliveredOn.Month == month)
            .Where(e => e.DeliveredOn.Year == year)
            .GroupBy(e => e.CustomerId)
            .Select(e => new
            {
                CustomerId = e.Key,
                TotalSpent = e.Sum(x => x.Price)
            });

        return await orderData.Union(expensesData)
            .Join(dbContext.Customers,
                o => o.CustomerId,
                c => c.IdentityId,
                (order, customer) => new { order, customer })
            .Select(x => new CustomerMonthlySpendingDto
            {
                CustomerId = x.customer.IdentityId,
                CustomerFullName = x.customer.Identity.FullName,
                Month = month,
                Year = year,
                CurrentBalance = x.customer.Budget.Balance,
                TotalSpent = x.order.TotalSpent
            }).ToListAsync();
    }
}
