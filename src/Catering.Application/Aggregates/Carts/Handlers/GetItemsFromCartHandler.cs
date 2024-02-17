using Catering.Application.Aggregates.Carts.Requests;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Carts.Handlers;

internal class GetItemsFromCartHandler : IRequestHandler<GetItemsFromTheCart, List<Item>>
{
    private readonly IItemRepository _itemRepository;

    public GetItemsFromCartHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public Task<List<Item>> Handle(GetItemsFromTheCart request, CancellationToken cancellationToken)
        => _itemRepository.GetItemsFromCartAsync(request.CustomerId);
}
