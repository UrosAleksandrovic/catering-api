using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Domain;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerReportsRepository
{
    Task<List<CustomerMonthlySpendingDto>> GetMonthlySendingReportAsync(YearAndMonth target);
}
