using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Carts.Requests;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Domain.Entities.ItemAggregate;
using MediatR;

namespace Catering.Application.Handlers;

internal class GetItemsFromCartHandler : IRequestHandler<GetItemsFromTheCart, List<Item>>
{
    private readonly IItemRepository _itemRepository;

    public GetItemsFromCartHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public Task<List<Item>> Handle(GetItemsFromTheCart request, CancellationToken cancellationToken)
    {
        return _itemRepository.GetItemsFromCartAsync(request.CustomerId);
    }
}
