using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public class GetItemsForPlacingOrder : IRequest<List<Item>>
{
    public string CustomerId { get; init; }
}
