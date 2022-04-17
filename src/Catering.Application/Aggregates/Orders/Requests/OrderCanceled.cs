using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public class OrderCanceled : INotification
{
    public string CustomerId { get; init; }
    public long OrderId { get; init; }
}
