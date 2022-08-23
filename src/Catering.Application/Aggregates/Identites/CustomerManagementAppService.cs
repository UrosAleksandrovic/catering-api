using AutoMapper;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Identites.Dtos;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Domain.Exceptions;

namespace Catering.Application.Aggregates.Identites;

internal class CustomerManagementAppService : ICustomerManagementAppService
{
    private readonly IIdentityRepository<Identity> _identityRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerManagementAppService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IIdentityRepository<Identity> identityRepository)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _identityRepository = identityRepository;
    }

    public async Task<string> CreateClientsCustomerAsync(CreateCustomerDto createCustomer, string creatorId)
    {
        await CheckIfInitiatorIsAdminAsync(creatorId, nameof(CreateClientsCustomerAsync));

        var customerExists = await _customerRepository.GetByIdentityEmailAsync(createCustomer.Email);
        if (customerExists != null)
            throw new ArgumentException("Client user with provided email already exists.");

        var customerIdentity = new Identity(createCustomer.Email, IdentityRole.Employee | IdentityRole.Client);
        var customerToCreate = new Customer(customerIdentity);

        await _identityRepository.CreateAsync(customerIdentity);
        await _customerRepository.CreateAsync(customerToCreate);

        return customerIdentity.Id;
    }

    public async Task<CustomerBudgetInfoDto> GetCustomerBudgetInfoAsync(string customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        return _mapper.Map<CustomerBudgetInfoDto>(customer.Budget);
    }

    public async Task<CustomerInfoDto> GetCustomerInfoAsync(string customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        return _mapper.Map<CustomerInfoDto>(customer);
    }

    private async Task<Customer> GetInitiatorAsync(string initiatorId)
    {
        var result = await _customerRepository.GetByIdAsync(initiatorId);

        if (result == default)
            throw new ArgumentException("Initiator does not exist.");

        return result;
    }

    private async Task CheckIfInitiatorIsAdminAsync(string initiatorId, string actionName)
    {
        var initiator = await GetInitiatorAsync(initiatorId);

        if (!initiator.Identity.Role.IsAdministrator())
            throw new IdentityRestrictionException(initiatorId, actionName);
    }

    public async Task<FilterResult<CustomerInfoDto>> GetFilteredAsync(CustomersFilter filter)
    {
        var result = new FilterResult<CustomerInfoDto>
        {
            PageIndex = filter.PageIndex,
            PageSize = filter.PageSize,
            TotalNumberOfPages = 0,
            Result = Enumerable.Empty<CustomerInfoDto>()
        };

        var (items, totalCount) = await _customerRepository.GetFilteredAsync(filter);
        result.TotalNumberOfPages = totalCount / filter.PageSize;
        result.Result = _mapper.Map<IEnumerable<CustomerInfoDto>>(items);

        return result;
    }
}
