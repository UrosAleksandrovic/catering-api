using AutoMapper;
using Catering.Application.Abstractions.Repositories;
using Catering.Application.Abstractions.Services;
using Catering.Application.Dtos.Item;
using Catering.Application.Filters;
using Catering.Domain.Builders;
using Catering.Domain.Entities.ItemAggregate;
using Catering.Domain.Exceptions;

namespace Catering.Application.Services;

public class ItemManagementAppService : IItemManagementAppService
{
    private readonly IItemRepository _itemRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public ItemManagementAppService(
        IItemRepository itemRepository,
        IMapper mapper,
        IOrderRepository orderRepository)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
        _orderRepository = orderRepository;
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

        var orders = _orderRepository.GetActiveOrdersForItemAsync(itemId);
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

    public async Task<short> GetUserRatingForItemAsync(Guid itemId, string userId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        var userRating = item.Ratings.SingleOrDefault(r => r.UserId == userId);

        return userRating == default ? (short)0 : userRating.Rating;
    }

    public async Task RateItemAsync(Guid itemId, string userId, short rating)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        item.AddOrChangeRating(userId, rating);
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
