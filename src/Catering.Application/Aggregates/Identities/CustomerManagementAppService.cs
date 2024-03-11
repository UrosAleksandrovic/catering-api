using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;
using Catering.Application.Validation;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.ErrorCodes;

namespace Catering.Application.Aggregates.Identities;

internal class CustomerManagementAppService : ICustomerManagementAppService
{
    private readonly IIdentityRepository<Identity> _identityRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidationProvider _validationProvider;

    public CustomerManagementAppService(
        ICustomerRepository customerRepository,
        IIdentityRepository<Identity> identityRepository,
        IValidationProvider validationProvider)
    {
        _customerRepository = customerRepository;
        _identityRepository = identityRepository;
        _validationProvider = validationProvider;
    }

    public async Task<Result<string>> CreateClientsCustomerAsync(CreateCustomerDto createCustomer, string creatorId)
    {
        if (await ValidateCustomerCreationAsync(createCustomer, creatorId) is var valResult && !valResult.IsSuccess)
            return Result.From<string>(valResult);

        var customerIdentity = new Identity(createCustomer.Email, IdentityRole.ClientEmployee, false);
        var customerToCreate = new Customer(customerIdentity);
        await _customerRepository.CreateAsync(customerToCreate, customerIdentity);

        return Result.Success(customerIdentity.Id);
    }

    private async Task<Result> ValidateCustomerCreationAsync(CreateCustomerDto createRequest, string creatorId)
    {
        if (_validationProvider.ValidateModel(createRequest) is var valResult && !valResult.IsSuccess)
            return Result.From<string>(valResult);

        if (await CheckIfInitiatorIsAdminAsync(creatorId) is var adminCheck && !adminCheck.IsSuccess)
            return Result.From<string>(adminCheck);

        var customerExists = await _customerRepository.GetByIdentityEmailAsync(createRequest.Email);
        if (customerExists != null)
            return Result.ValidationError(IdentityErrorCodes.IDENTITY_ALREADY_EXISTS);

        return Result.Success();
    }

    private async Task<Result> CheckIfInitiatorIsAdminAsync(string initiatorId)
    {
        var initiator = await _identityRepository.GetByIdAsync(initiatorId);

        if (initiator == default)
            return Result.ValidationError(IdentityErrorCodes.INITIATOR_IDENTITY_NOT_FOUND);

        if (!initiator.Role.IsAdministrator())
            return Result.ValidationError(IdentityErrorCodes.FORBIDDEN_ACTION);

        return Result.Success();
    }
}
