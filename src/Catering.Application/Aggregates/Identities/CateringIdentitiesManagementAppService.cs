using AutoMapper;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Aggregates.Identities.Notifications;
using Catering.Application.Security;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Builders;
using Catering.Domain.ErrorCodes;
using Catering.Domain.Exceptions;
using MediatR;

namespace Catering.Application.Aggregates.Identities;

internal class CateringIdentitiesManagementAppService : ICateringIdentitiesManagementAppService
{
    private readonly ICateringIdentitiesRepository _cateringIdentitiesRepository;
    private readonly IIdentityRepository<Identity> _identityRepository;
    private readonly IMediator _publisher;
    private readonly IDataProtector _dataProtector;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CateringIdentitiesManagementAppService(
        ICateringIdentitiesRepository cateringIdentitiesRepository,
        IMediator publisher,
        IIdentityRepository<Identity> identityRepository,
        IDataProtector dataProtector,
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _cateringIdentitiesRepository = cateringIdentitiesRepository;
        _publisher = publisher;
        _identityRepository = identityRepository;
        _dataProtector = dataProtector;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task SendIdentityInvitationAsync(string creatorId, CreateIdentityInvitationDto createRequest)
    {
        var creator = await _identityRepository.GetByIdAsync(creatorId);
        if (creator != null && creator.HasRole(IdentityRoleExtensions.GetClientAdministrator()))
            throw new IdentityRestrictionException(creatorId, "User Invitation");

        var identityExists = await _cateringIdentitiesRepository.GetByEmailAsync(createRequest.Email);
        if (identityExists != null)
            throw new IdentityAlreadyExists();

        var invtitation = new IdentityInvitationBuilder()
            .HasEmail(createRequest.Email)
            .HasFullName(createRequest.FirstName, createRequest.LastName)
            .HasFutureRole(createRequest.FutureRole, createRequest.IsCustomer)
            .Build();

        await _cateringIdentitiesRepository.CreateInvitationAsync(invtitation);

        await _publisher.Publish(new IdentityInvitationCreated { InvitationId = invtitation.Id });
    }

    public async Task AcceptInvitationAsync(string invitationId, string newPassword)
    {
        var invitation = await _cateringIdentitiesRepository.GetInvitationByIdAsync(invitationId);
        if (invitation == null)
            throw new CateringException(IdentityErrorCodes.INVITATION_NOT_FOUND);


        var (identity, customer) = invitation.AcceptInvitation(_dataProtector.Hash(newPassword));
        await _identityRepository.CreateAsync(identity);
        if (customer != null)
            await _customerRepository.CreateAsync(customer);

        await _cateringIdentitiesRepository.RemoveInvitationAsync(invitation);
    }

    public async Task<IdentityInfoDto> GetIdentityInfoAsync(string identityId)
    {
        var identity = await _identityRepository.GetByIdAsync(identityId);

        return _mapper.Map<IdentityInfoDto>(identity);
    }

    public async Task<IdentityPermissionsDto> GetIdentityPermissionsAsync(string identityId)
    {
        var identity = await _identityRepository.GetByIdAsync(identityId);

        return new IdentityPermissionsDto
        {
            Permissions = identity.Role.GetFromRole(),
            Role = identity.Role
        };
    }
}
