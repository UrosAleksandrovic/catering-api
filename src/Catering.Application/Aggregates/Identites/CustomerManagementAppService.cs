using AutoMapper;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Exceptions;

namespace Catering.Application.Aggregates.Identites;

internal class CustomerManagementAppService : ICustomerManagementAppService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerManagementAppService(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<string> CreateCompanyCustomer(CreateCustomerDto createCustomer, string creatorId)
    {
        await CheckIfInitiatorIsAdminAsync(creatorId, nameof(CreateCompanyCustomer));

        var customerExists = await _customerRepository.GetByEmailAsync(createCustomer.Email);
        if (customerExists != null)
            throw new ArgumentException("Company user with provided email already exists.");

        var customerToCreate = new Customer(createCustomer.Email, IdentityRole.CompanyEmployee);
        await _customerRepository.CreateAsync(customerToCreate);

        return customerToCreate.Id;
    }

    public async Task<CustomerBudgetInfoDto> GetCustomerBudgetInfo(string customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        return _mapper.Map<CustomerBudgetInfoDto>(customer.Budget);
    }

    public async Task<CustomerInfoDto> GetCustomerInfo(string customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        return _mapper.Map<CustomerInfoDto>(customer);
    }

    private async Task<Identity> GetInitiatorAsync(string initiatorId)
    {
        var result = await _customerRepository.GetByIdAsync(initiatorId);

        if (result == default)
            throw new ArgumentException("Initiator does not exist.");

        return result;
    }

    private async Task CheckIfInitiatorIsAdminAsync(string initiatorId, string actionName)
    {
        var initiator = await GetInitiatorAsync(initiatorId);

        if (!initiator.IsAdministrator)
            throw new IdentityRestrictionException(initiatorId, actionName);
    }
}
