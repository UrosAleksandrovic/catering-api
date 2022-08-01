﻿using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using static Catering.Application.Mailing.Emails.TemplateNamesConstants;

namespace Catering.Application.Mailing.Emails.Handlers;

internal class OrderCanceledEmailHandler : INotificationHandler<OrderCanceled>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailSender _emailSender;
    private readonly IEmailBuilder _emailBuilder;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<OrderCanceledEmailHandler> _logger;

    public OrderCanceledEmailHandler(
        IEmailRepository emailRepository,
        IEmailSender emailSender,
        IOrderRepository orderRepository,
        IEmailBuilder emailBuilder,
        ICustomerRepository customerRepository,
        IMenuRepository menuRepository,
        ILogger<OrderCanceledEmailHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
        _orderRepository = orderRepository;
        _emailBuilder = emailBuilder;
        _customerRepository = customerRepository;
        _menuRepository = menuRepository;
        _logger = logger;
    }

    public async Task Handle(OrderCanceled notification, CancellationToken cancellationToken)
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
        catch(Exception e)
        {
            _logger.LogError(e, "Failed to send an email.");
        }
    }

    private async Task<CateringEmail> GenerateEmailAsync(OrderCanceled notification)
    {
        CateringEmail generatedEmail;
        try
        {
            var template = await _emailRepository.GetTemplateAsync(OrderCanceledCustomer);
            var order = await _orderRepository.GetByIdAsync(notification.OrderId);
            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            var menu = await _menuRepository.GetByIdAsync(order.MenuId);

            _emailBuilder.HasTitle($"Order #{order.Id}");
            _emailBuilder.HasEmailTemplate(template, new { Order = order, Menu = menu, Customer = customer });
            _emailBuilder.HasRecepient(customer.Email);
            generatedEmail = _emailBuilder.Build();
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Failed to generate email for newly placed order.");
            return default;
        }

        return generatedEmail;
    }
}
