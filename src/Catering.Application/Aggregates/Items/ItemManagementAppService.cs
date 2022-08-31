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

    public async Task<FilterResult<ItemInfoDto>> GetFilteredAsync(ItemsFilter itemFilters, string requestorId)
    {
        var result = new FilterResult<ItemInfoDto>
        {
            PageIndex = itemFilters.PageIndex,
            PageSize = itemFilters.PageSize,
            TotalNumberOfPages = 0,
            Result = Enumerable.Empty<ItemInfoDto>()
        };

        var request = new GetIdentityForMenuId { IdentityId = requestorId, MenuId = itemFilters.MenuId };
        var requestor = await _publisher.Send(request);
        if (requestor == default)
            return result;

        var (items, totalCount) = await _itemRepository.GetFilteredAsync(itemFilters);
        result.TotalNumberOfPages = totalCount / itemFilters.PageSize;
        result.Result = _mapper.Map<IEnumerable<ItemInfoDto>>(items);

        return result;
    }

    public async Task<ItemInfoDto> GetItemByIdAsync(Guid itemId, string requestorId)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            return default;

        var request = new GetIdentityForMenuId { IdentityId = requestorId, MenuId = item.MenuId };
        var creator = await _publisher.Send(request);

        return creator == default ? default : _mapper.Map<ItemInfoDto>(item);
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

    public async Task UpdateItemAsync(Guid itemId, UpdateItemDto updateRequest)
    {
        var item = await _itemRepository.GetByIdAsync(itemId);
        if (item == default)
            throw new KeyNotFoundException();

        item.UpdateAllCategories(updateRequest.Categories);
        item.UpdateAllIngredients(updateRequest.Ingredients);
        item.EditGeneralData(updateRequest.Name, updateRequest.Description, updateRequest.Price);

        await _itemRepository.UpdateAsync(item);
    }

    public async Task<List<ItemsLeaderboardDto>> GetMostOrderedFromTheMenuAsync(int top, Guid menuId)
    {
        var result = await _itemRepository.GetMostOrderedFromTheMenuAsync(top, menuId);

        List<ItemsLeaderboardDto> dtoResult = new();
        foreach (var item in result)
            dtoResult.Add(new ItemsLeaderboardDto { ItemInfo = _mapper.Map<ItemInfoDto>(item.item), EvaluatedValue = item.numOfOrders });

        return dtoResult;
    }
}
