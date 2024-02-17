using Catering.Domain.Aggregates.Order;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

public class GetActiveOrdersOfItem : IRequest<List<Order>>
{
    public Guid ItemId { get; init; }
}
