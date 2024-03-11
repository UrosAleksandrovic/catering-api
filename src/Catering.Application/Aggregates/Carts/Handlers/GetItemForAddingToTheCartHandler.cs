using Catering.Application.Aggregates.Carts.Requests;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Domain.Aggregates.Item;
using MediatR;

namespace Catering.Application.Aggregates.Carts.Handlers;

internal class GetItemForAddingToTheCartHandler : IRequestHandler<GetItemForAddingToTheCart, Item>
{
    private readonly IItemRepository _itemRepository;

    public GetItemForAddingToTheCartHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public Task<Item> Handle(GetItemForAddingToTheCart request, CancellationToken cancellationToken)
        => _itemRepository.GetByMenuAndIdAsync(request.MenuId, request.ItemId);
}
