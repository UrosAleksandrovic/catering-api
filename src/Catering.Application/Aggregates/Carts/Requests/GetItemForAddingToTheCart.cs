using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Carts.Requests;

public record GetItemForAddingToTheCart(Guid MenuId, Guid ItemId) : IRequest<Item>;
