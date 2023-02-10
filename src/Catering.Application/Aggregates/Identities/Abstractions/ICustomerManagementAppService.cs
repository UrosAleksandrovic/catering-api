using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerManagementAppService
{
    Task<string> CreateClientsCustomerAsync(CreateCustomerDto createCustomer, string creatorId);
    Task<CustomerInfoDto> GetCustomerInfoAsync(string customerId);
    Task<CustomerBudgetInfoDto> GetCustomerBudgetInfoAsync(string customerId);
    Task<FilterResult<CustomerInfoDto>> GetFilteredAsync(CustomersFilter filter);
}
