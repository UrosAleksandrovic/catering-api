using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerReportsRepository
{
    Task<List<CustomerMonthlySpendingDto>> GetMonthlySendingReportAsync(int month, int year);
}
