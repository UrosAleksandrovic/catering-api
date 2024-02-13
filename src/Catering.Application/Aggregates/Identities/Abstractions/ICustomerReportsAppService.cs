using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerReportsAppService
{
    Task<List<CustomerMonthlySpendingDto>> GetMonthlySendingAsync(int month, int year);
}
