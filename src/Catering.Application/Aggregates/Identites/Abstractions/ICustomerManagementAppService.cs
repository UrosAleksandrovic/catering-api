using Catering.Application.Aggregates.Identites.Dtos;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface ICustomerManagementAppService
{
    Task<string> CreateCompanyCustomerAsync(CreateCustomerDto createCustomer, string creatorId);
    Task<CustomerInfoDto> GetCustomerInfoAsync(string customerId);
    Task<CustomerBudgetInfoDto> GetCustomerBudgetInfoAsync(string customerId);
    Task<FilterResult<CustomerInfoDto>> GetFilteredAsync(CustomersFilter filter);
}
