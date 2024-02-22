using AutoMapper;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Requests;
using Catering.Domain.Builders;
using MediatR;

namespace Catering.Application.Aggregates.Items;

internal class ItemManagementAppService : IItemManagementAppService
{
    private readonly IItemRepository _itemRepository;
    private readonly IMediator _publisher;
    private readonly IMapper _mapper;

    public ItemManagementAppService(
        IItemRepository itemRepository,
        IMapper mapper,
        IMediator publisher)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Guid> CreateItemAsync(Guid menuId, CreateItemDto createRequest)
    {
        var itemToCreate = new ItemBuilder()
            .HasMenu(menuId)
            .HasIngredients(createRequest.Ingredients)
            .HasCategories(createRequest.Categories)
            .HasGeneralData(createRequest.Name, createRequest.Description, createRequest.Price)
            .Build();

        var createdItem = await _itemRepository.CreateAsync(itemToCreate);

        return createdItem.Id;
    }

    public async Task DeleteItemAsync(Guid menuId, Guid itemId)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            throw new KeyNotFoundException();

        var orders = await _publisher.Send(new GetActiveOrdersOfItem { ItemId = itemId });
        if (orders.Any())
            throw new InvalidOperationException();

        item.MarkAsDeleted();

        await _itemRepository.UpdateAsync(item);
    }

    public async Task<ItemInfoDto> GetItemByIdAsync(Guid menuId, Guid itemId, string requesterId)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            return default;

        var request = new GetIdentityForMenuId { IdentityId = requesterId, MenuId = item.MenuId };
        var creator = await _publisher.Send(request);

        return creator == default ? default : _mapper.Map<ItemInfoDto>(item);
    }

    public async Task<short> GetCustomerRatingForItemAsync(Guid menuId, Guid itemId, string customerId)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            throw new KeyNotFoundException();

        var customerRating = item.Ratings.SingleOrDefault(r => r.CustomerId == customerId);

        return customerRating?.Rating ?? 0;
    }

    public async Task RateItemAsync(Guid menuId, Guid itemId, string customerId, short rating)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            throw new KeyNotFoundException();

        item.AddOrChangeRating(customerId, rating);
        await _itemRepository.UpdateAsync(item);
    }

    public async Task UpdateItemAsync(Guid menuId, Guid itemId, UpdateItemDto updateRequest)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            throw new KeyNotFoundException();

        item.UpdateAllCategories(updateRequest.Categories);
        item.UpdateAllIngredients(updateRequest.Ingredients);
        item.EditGeneralData(updateRequest.Name, updateRequest.Description, updateRequest.Price);

        await _itemRepository.UpdateAsync(item);
    }
}
