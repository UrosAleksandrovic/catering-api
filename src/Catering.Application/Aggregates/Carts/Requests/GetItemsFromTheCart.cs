using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Carts.Requests;

public record GetItemsFromTheCart(string CustomerId) : IRequest<List<Item>>;
