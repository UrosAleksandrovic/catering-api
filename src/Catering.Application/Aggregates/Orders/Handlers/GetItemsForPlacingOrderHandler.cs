using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class GetItemsForPlacingOrderHandler : IRequestHandler<GetItemsForPlacingOrder, List<Item>>
{
    private readonly IItemRepository _itemRepository;

    public GetItemsForPlacingOrderHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public Task<List<Item>> Handle(GetItemsForPlacingOrder request, CancellationToken cancellationToken)
        => _itemRepository.GetItemsFromCartAsync(request.CustomerId);
}
