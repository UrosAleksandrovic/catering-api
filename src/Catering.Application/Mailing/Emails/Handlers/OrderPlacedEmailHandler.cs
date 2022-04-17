using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using MediatR;
using static Catering.Application.Mailing.Emails.TemplateNamesConstants;

namespace Catering.Application.Mailing.Emails.Handlers;

internal class OrderPlacedEmailHandler : INotificationHandler<OrderPlaced>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailSender _emailSender;

    public OrderPlacedEmailHandler(
        IEmailRepository emailRepository,
        IEmailSender emailSender,
        IOrderRepository orderRepository)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
        _orderRepository = orderRepository;
    }

    public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {
        var template = await _emailRepository.GetTemplateAsync(OrderPlacedRestourant);
        var orderMenu = await _orderRepository.GetOrderMenuAsync(notification.OrderId);

        var email = new EmailBuilder()
            .HasEmailTemplate(template)
            .HasRecepient(orderMenu.Contact.Address)
            .Build();
        //TOOD: ADD parameters when design of html template is complete

        await _emailSender.SendAsync(email);
    }
}
