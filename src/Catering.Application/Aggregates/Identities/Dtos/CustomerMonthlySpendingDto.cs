using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities.Dtos;

public class CustomerMonthlySpendingDto
{
    public string CustomerId { get; set; }
    public FullName CustomerFullName { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal TotalSpent { get; set; }
}
