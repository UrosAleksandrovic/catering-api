using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Orders.Notifications;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class OrderPlacedHandler : INotificationHandler<OrderPlaced>
{
    private readonly ICartRepository _cartRepository;

    public OrderPlacedHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public Task Handle(OrderPlaced notification, CancellationToken cancellationToken)
        => _cartRepository.DeleteAsync(notification.CustomerId);
}
