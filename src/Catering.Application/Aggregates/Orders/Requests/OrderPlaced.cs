using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

internal class OrderPlaced : INotification
{
    public string CustomerId { get; init; }
    public long OrderId { get; init; }
}
