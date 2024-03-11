using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Aggregates.Identities.Notifications;
using Catering.Application.Results;
using Catering.Application.Security;
using Catering.Application.Validation;
using Catering.Domain.Aggregates.Identity;
using Catering.Domain.Builders;
using Catering.Domain.ErrorCodes;
using MediatR;

namespace Catering.Application.Aggregates.Identities;

internal class CateringIdentitiesManagementAppService : ICateringIdentitiesManagementAppService
{
    private readonly ICateringIdentitiesRepository _cateringIdentitiesRepository;
    private readonly IIdentityRepository<Identity> _identityRepository;
    private readonly IMediator _publisher;
    private readonly IDataProtector _dataProtector;
    private readonly IValidationProvider _validationProvider;

    public CateringIdentitiesManagementAppService(
        ICateringIdentitiesRepository cateringIdentitiesRepository,
        IMediator publisher,
        IIdentityRepository<Identity> identityRepository,
        IDataProtector dataProtector,
        IValidationProvider validationProvider)
    {
        _cateringIdentitiesRepository = cateringIdentitiesRepository;
        _publisher = publisher;
        _identityRepository = identityRepository;
        _dataProtector = dataProtector;
        _validationProvider = validationProvider;
    }

    public async Task<Result> SendIdentityInvitationAsync(string creatorId, CreateIdentityInvitationDto createRequest)
    {
        if (await _validationProvider.ValidateModelAsync(createRequest) is var valRes && !valRes.IsSuccess)
            return valRes;

        var creator = await _identityRepository.GetByIdAsync(creatorId);
        if (creator != null && creator.HasRole(IdentityRole.ClientAdmin))
            return Result.ValidationError(IdentityErrorCodes.INVALID_CREATOR_ROLE);

        var identityExists = await _cateringIdentitiesRepository.GetByEmailAsync(createRequest.Email);
        if (identityExists != null)
            return Result.ValidationError(IdentityErrorCodes.IDENTITY_ALREADY_EXISTS);

        var invtitation = new IdentityInvitationBuilder()
            .HasEmail(createRequest.Email)
            .HasFullName(createRequest.FirstName, createRequest.LastName)
            .HasFutureRole(createRequest.FutureRole, createRequest.IsCustomer)
            .Build();

        await _cateringIdentitiesRepository.CreateInvitationAsync(invtitation);

        await _publisher.Publish(new IdentityInvitationCreated { InvitationId = invtitation.Id });
        return Result.Success();
    }

    public async Task<Result> AcceptInvitationAsync(string invitationId, string newPassword)
    {
        var invitation = await _cateringIdentitiesRepository.GetInvitationByIdAsync(invitationId);
        if (invitation == null)
            return Result.ValidationError(IdentityErrorCodes.INVITATION_NOT_FOUND);

        var (identity, customer) = invitation.AcceptInvitation(_dataProtector.Hash(newPassword));
        await _identityRepository.CompleteInvitationAsync(identity, customer, invitation);

        return Result.Success();
    }
}
