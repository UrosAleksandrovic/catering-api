using AutoMapper;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Requests;
using Catering.Domain.Builders;
using Catering.Domain.Entities.ItemAggregate;
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

    public async Task<Guid> CreateItemAsync(CreateItemDto createRequest)
    {
        var itemToCreate = new ItemBuilder()
            .HasMenu(createRequest.MenuId)
            .HasIngredients(createRequest.Ingredients)
            .HasCategories(createRequest.Categories)
            .HasGeneralData(createRequest.Name, createRequest.Description, createRequest.Price)
            .Build();

        var createdItem = await _itemRepository.CreateAsync(itemToCreate);

        return createdItem.Id;
    }

    public async Task DeleteItemAsync(Guid itemId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        var orders = await _publisher.Send(new GetActiveOrdersOfItem { ItemId = itemId });
        if (orders.Any())
            throw new InvalidOperationException();

        item.MarkAsDeleted();

        await _itemRepository.UpdateAsync(item);
    }

    public async Task<FilterResult<ItemInfoDto>> GetFilteredAsync(ItemsFilter itemFilters)
    {
        var items = await _itemRepository.GetFilteredAsync(itemFilters);

        return new FilterResult<ItemInfoDto>
        {
            PageIndex = items.PageIndex,
            Result = _mapper.Map<IEnumerable<ItemInfoDto>>(items.Result),
            PageSize = items.PageSize,
            TotalNumberOfPages = items.TotalNumberOfPages
        };
    }

    public async Task<ItemInfoDto> GetItemByIdAsync(Guid itemId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);

        return item == default ? default : _mapper.Map<ItemInfoDto>(item);
    }

    public async Task<short> GetCustomerRatingForItemAsync(Guid itemId, string customerId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        var customerRating = item.Ratings.SingleOrDefault(r => r.CustomerId == customerId);

        return customerRating == default ? (short)0 : customerRating.Rating;
    }

    public async Task RateItemAsync(Guid itemId, string customerId, short rating)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        item.AddOrChangeRating(customerId, rating);
        await _itemRepository.UpdateAsync(item);
    }

    public async Task<Guid> UpdateItemAsync(Guid itemId, UpdateItemDto updateRequest)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        UpdateItemCategories(item, updateRequest.Categories);
        UpdateItemIngredients(item, updateRequest.Ingredients);
        item.EditGeneralData(updateRequest.Name, updateRequest.Description, updateRequest.Price);

        await _itemRepository.UpdateAsync(item);

        return item.Id;
    }

    private void UpdateItemCategories(Item item, IEnumerable<string> updateRequestCategories)
    {
        var categoriesToRemove = item.Categories.Except(updateRequestCategories);
        item.RemoveCategories(categoriesToRemove);

        item.AddCategories(updateRequestCategories);
    }

    private void UpdateItemIngredients(Item item, IEnumerable<string> updateRequestIngredients)
    {
        var categoriesToRemove = item.Categories.Except(updateRequestIngredients);
        item.RemoveCategories(categoriesToRemove);

        item.AddCategories(updateRequestIngredients);
    }
}
