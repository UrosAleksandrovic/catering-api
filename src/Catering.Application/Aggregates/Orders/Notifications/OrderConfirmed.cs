using MediatR;

namespace Catering.Application.Aggregates.Orders.Notifications;

public class OrderConfirmed : INotification
{
    public string CustomerId { get; init; }
    public long OrderId { get; init; }
}
