using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Identities.Abstractions;

public interface ICustomerManagementAppService
{
    Task<Result<string>> CreateClientsCustomerAsync(CreateCustomerDto createCustomer, string creatorId);
}
