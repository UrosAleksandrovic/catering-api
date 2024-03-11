using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using static Catering.Application.Mailing.Emails.TemplateNamesConstants;

namespace Catering.Application.Mailing.Emails.Handlers;

internal class OrderPlacedRestaurantEmailHandler : INotificationHandler<OrderPlaced>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IOrdersQueryRepository _orderQueryRepository;
    private readonly IMenusQueryRepository _menuQueryRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailBuilder _emailBuilder;
    private readonly ILogger<OrderPlacedRestaurantEmailHandler> _logger;

    public OrderPlacedRestaurantEmailHandler(
        IEmailRepository emailRepository,
        IEmailSender emailSender,
        IOrdersQueryRepository orderRepository,
        IMenusQueryRepository menuRepository,
        IEmailBuilder emailBuilder,
        ILogger<OrderPlacedRestaurantEmailHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
        _orderQueryRepository = orderRepository;
        _emailBuilder = emailBuilder;
        _menuQueryRepository = menuRepository;
        _logger = logger;
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
            var order = await _orderQueryRepository.GetByIdAsync(notification.OrderId);
            var contactEmail = await _menuQueryRepository.GetContactEmailAsync(order.MenuId);

            _emailBuilder.HasTitle($"New order arrived. (#{order.Id})");
            _emailBuilder.HasEmailTemplate(template, new { Order = order });
            _emailBuilder.HasRecepient(contactEmail);

            generatedEmail = _emailBuilder.Build();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Failed to generate email");
            return default;
        }

        return generatedEmail;
    }
}
