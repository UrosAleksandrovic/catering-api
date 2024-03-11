using Catering.Domain.Aggregates.Cart;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public record GetCartFromCustomer(string CustomerId) : IRequest<Cart>;
