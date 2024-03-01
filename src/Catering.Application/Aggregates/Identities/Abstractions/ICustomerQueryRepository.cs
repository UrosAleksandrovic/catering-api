using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerQueryRepository
{
    Task<CustomerBudgetInfoDto> GetCustomerBudgetAsync(string customerId);
    Task<CustomerInfoDto> GetCustomerInfoAsync(string customerId);
}
