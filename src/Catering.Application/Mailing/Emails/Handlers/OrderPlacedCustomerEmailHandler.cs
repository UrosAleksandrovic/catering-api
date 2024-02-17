using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using Catering.Domain.Aggregates.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using static Catering.Application.Mailing.Emails.TemplateNamesConstants;

namespace Catering.Application.Mailing.Emails.Handlers;

internal class OrderPlacedCustomerEmailHandler : INotificationHandler<OrderPlaced>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailBuilder _emailBuilder;
    private readonly IIdentityRepository<Identity> _identityRepository;
    private readonly ILogger<OrderPlacedCustomerEmailHandler> _logger;

    public OrderPlacedCustomerEmailHandler(
        IEmailRepository emailRepository,
        IEmailSender emailSender,
        IOrderRepository orderRepository,
        IEmailBuilder emailBuilder,
        ILogger<OrderPlacedCustomerEmailHandler> logger,
        IIdentityRepository<Identity> identityRepository)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
        _orderRepository = orderRepository;
        _emailBuilder = emailBuilder;
        _logger = logger;
        _identityRepository = identityRepository;
    }

    public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {
        CateringEmail emailToSend = await GenerateEmailAsync(notification);
        if (emailToSend == default)
            return;

        try
        {
            var isSent = await _emailSender.SendAsync(emailToSend);
            if (!isSent)
                await _emailRepository.SaveAsFailedEmailAsync(emailToSend);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Failed to send email");
        }
    }

    private async Task<CateringEmail> GenerateEmailAsync(OrderPlaced notification)
    {
        CateringEmail generatedEmail;
        try
        {
            var template = await _emailRepository.GetTemplateAsync(OrderPlacedRestaurant);
            var order = await _orderRepository.GetByIdAsync(notification.OrderId);
            var customerIdentity = await _identityRepository.GetByIdAsync(order.CustomerId);

            _emailBuilder.HasTitle($"Order #{order.Id}")
                .HasEmailTemplate(template, new { Order = order })
                .HasRecepient(customerIdentity.Email);

            generatedEmail = _emailBuilder.Build();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to send an email");
            return default;
        }
        return generatedEmail;
    }
}
