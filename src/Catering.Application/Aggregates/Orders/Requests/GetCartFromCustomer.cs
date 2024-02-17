using Catering.Domain.Aggregates.Cart;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public class GetCartFromCustomer : IRequest<Cart>
{
    public string CustomerId { get; init; }
}
