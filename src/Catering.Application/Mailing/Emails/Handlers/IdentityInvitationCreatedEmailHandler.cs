using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catering.Application.Mailing.Emails.Handlers;

internal class IdentityInvitationCreatedEmailHandler : INotificationHandler<IdentityInvitationCreated>
{
    private readonly ICateringIdentitiesRepository _cateringIdentityRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailBuilder _emailBuilder;
    private readonly ILogger<IdentityInvitationCreatedEmailHandler> _logger;

    private const string _urlToFollow = "";

    public IdentityInvitationCreatedEmailHandler(
        IEmailRepository emailRepository,
        IEmailSender emailSender,
        IEmailBuilder emailBuilder,
        ICateringIdentitiesRepository cateringIdentityRepository,
        ILogger<IdentityInvitationCreatedEmailHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
        _emailBuilder = emailBuilder;
        _cateringIdentityRepository = cateringIdentityRepository;
        _logger = logger;
    }

    public async Task Handle(IdentityInvitationCreated notification, CancellationToken cancellationToken)
    {
        var emailToSend = await GenerateEmailAsync(notification);
        if (emailToSend == default)
            return;

        try
        {
            var isSent = await _emailSender.SendAsync(emailToSend);
            if (!isSent)
                await _emailRepository.SaveAsFailedEmailAsync(emailToSend);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send an email.");
        }
    }

    private async Task<CateringEmail> GenerateEmailAsync(IdentityInvitationCreated notification)
    {
        CateringEmail generatedEmail;
        try
        {   
            var template = await _emailRepository.GetTemplateAsync(TemplateNamesConstants.IdentityInvitationCreated);
            var invitation = await _cateringIdentityRepository.GetInvitationByIdAsync(notification.InvitationId);

            _emailBuilder.HasTitle($"Invitation To Catering App.");
            _emailBuilder.HasEmailTemplate(template, new { 
                FirstName = invitation.FullName.FirstName, 
                LastName = invitation.FullName.LastName,
                UrlToFollow = _urlToFollow + invitation.Id });
            _emailBuilder.HasRecepient(invitation.Email);
            generatedEmail = _emailBuilder.Build();

            await _emailSender.SendAsync(generatedEmail);
        }   
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to generate email for newly placed order.");
            return default;
        }

        return generatedEmail;
    }
}
