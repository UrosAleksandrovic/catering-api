using Catering.Domain.Entities.CartAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public class GetCartFromCustomer : IRequest<Cart>
{
    public string CustomerId { get; init; }
}
