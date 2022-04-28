using AutoMapper;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Catering.Domain.Entities.IdentityAggregate;

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

    public async Task<string> CreateCompanyCustomer(CreateCustomerDto createCustomer)
    {
        var customerExists = await _customerRepository.GetByEmailAsync(createCustomer.Email);
        if (customerExists != null)
            throw new ArgumentException(nameof(createCustomer.Email));

        var customerToCreate = new Customer(createCustomer.Email, ConvertToPermissions(createCustomer));
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

    public IdentityPermissions ConvertToPermissions(CreateCustomerDto createCustomer)
    {
        //TODO: Depends on permissions. Get the list of persmissions from Tamara
        return IdentityPermissions.RestourantEmployee;
    }
}
