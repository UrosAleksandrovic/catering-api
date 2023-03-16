using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Aggregates.Identities.Notifications;
using Catering.Application.Security;
using Catering.Domain.Builders;
using Catering.Domain.Entities.IdentityAggregate;
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

    public CateringIdentitiesManagementAppService(
        ICateringIdentitiesRepository cateringIdentitiesRepository,
        IMediator publisher,
        IIdentityRepository<Identity> identityRepository,
        IDataProtector dataProtector,
        ICustomerRepository customerRepository)
    {
        _cateringIdentitiesRepository = cateringIdentitiesRepository;
        _publisher = publisher;
        _identityRepository = identityRepository;
        _dataProtector = dataProtector;
        _customerRepository = customerRepository;
    }

    public async Task SendIdentityInvitationAsync(string creatorId, CreateIdentityInvitationDto createRequest)
    {
        var creator = await _identityRepository.GetByIdAsync(creatorId);
        if (creator != null && creator.HasRole(IdentityRoleExtensions.GetClientAdministrator()))
            throw new IdentityRestrictionException(creatorId, "User Invitation");

        var identityExists = await _cateringIdentitiesRepository.GetByEmailAsync(createRequest.Email);
        if (identityExists != null)
            throw new ArgumentException("Identity with provided email already exists.");

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
            throw new CateringException("Invitation does not exist!");


        var (identity, customer) = invitation.AcceptInvitation(_dataProtector.Hash(newPassword));
        await _identityRepository.CreateAsync(identity);
        if (customer != null)
            await _customerRepository.CreateAsync(customer);

        await _cateringIdentitiesRepository.RemoveInvitationAsync(invitation);
    }
}
