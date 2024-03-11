using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Menus.Notifications;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Handlers;

internal class MenuDeletedHandler : INotificationHandler<MenuDeleted>
{
    private readonly IItemRepository _itemRepository;
    private readonly ICartRepository _cartRepository;

    public MenuDeletedHandler(IItemRepository itemrepository, ICartRepository cartRepository)
    {
        _itemRepository = itemrepository;
        _cartRepository = cartRepository;
    }

    public async Task Handle(MenuDeleted notification, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetItemsFromMenuAsync(notification.MenuId);

        items.ForEach(item => item.MarkAsDeleted());

        await _itemRepository.UpdateRangeAsync(items);
        await _cartRepository.DeleteItemsWithMenuAsync(notification.MenuId);
    }
}
