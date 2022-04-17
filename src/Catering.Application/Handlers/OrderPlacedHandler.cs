using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Application.Mailing.Emails;
using MediatR;

namespace Catering.Application.Handlers;

internal class OrderPlacedHandler : INotificationHandler<OrderPlaced>
{
    private readonly ICartRepository _cartRepository;

    public OrderPlacedHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
    {
        await _cartRepository.DeleteAsync(notification.CustomerId);
    }
}
