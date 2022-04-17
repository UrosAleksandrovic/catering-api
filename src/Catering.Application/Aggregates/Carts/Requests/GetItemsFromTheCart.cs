using Catering.Domain.Entities.ItemAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Carts.Requests;

public class GetItemsFromTheCart : IRequest<List<Item>>
{
    public string CustomerId { get; init; }
}
