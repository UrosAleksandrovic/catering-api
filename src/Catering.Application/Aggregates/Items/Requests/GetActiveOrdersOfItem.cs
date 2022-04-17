using Catering.Domain.Entities.OrderAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

public class GetActiveOrdersOfItem : IRequest<List<Order>>
{
    public Guid ItemId { get; init; }
}
