using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

public record GetItemsForPlacingOrder(string CustomerId) : IRequest<List<Item>>;
