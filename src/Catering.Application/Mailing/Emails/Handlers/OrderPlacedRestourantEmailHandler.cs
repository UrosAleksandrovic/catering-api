using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using static Catering.Application.Mailing.Emails.TemplateNamesConstants;

namespace Catering.Application.Mailing.Emails.Handlers;

internal class OrderPlacedRestourantEmailHandler : INotificationHandler<OrderPlaced>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailBuilder _emailBuilder;
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<OrderPlacedRestourantEmailHandler> _logger;

    public OrderPlacedRestourantEmailHandler(
        IEmailRepository emailRepository,
        IEmailSender emailSender,
        IOrderRepository orderRepository,
        IEmailBuilder emailBuilder,
        IMenuRepository menuRepository,
        ILogger<OrderPlacedRestourantEmailHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
        _orderRepository = orderRepository;
        _emailBuilder = emailBuilder;
        _menuRepository = menuRepository;
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
            var template = await _emailRepository.GetTemplateAsync(OrderPlacedRestourant);
            var order = await _orderRepository.GetByIdAsync(notification.OrderId);
            var menu = await _menuRepository.GetByIdAsync(order.MenuId);

            _emailBuilder.HasTitle($"New order arrived. (#{order.Id})");
            _emailBuilder.HasEmailTemplate(template, new { Order = order });
            _emailBuilder.HasRecepient(menu.Contact.Email);

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
