using Catering.Domain.Aggregates.Order;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

public record GetActiveOrdersOfItem(Guid ItemId) : IRequest<List<Order>>;
