using AutoMapper;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Requests;
using Catering.Application.Results;
using Catering.Application.Validation;
using Catering.Domain.Builders;
using Catering.Domain.ErrorCodes;
using MediatR;

namespace Catering.Application.Aggregates.Items;

internal class ItemManagementAppService : IItemManagementAppService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidationProvider _validationProvider;
    private readonly IMediator _publisher;
    private readonly IMapper _mapper;

    public ItemManagementAppService(
        IItemRepository itemRepository,
        IValidationProvider validationProvider,
        IMapper mapper,
        IMediator publisher)
    {
        _itemRepository = itemRepository;
        _validationProvider = validationProvider;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<Result<ItemsIdDto>> CreateItemAsync(Guid menuId, CreateItemDto createRequest)
    {
        if (await _validationProvider.ValidateModelAsync(createRequest) is var valRes && !valRes.IsSuccess)
            return Result.From<ItemsIdDto>(valRes);

        var itemToCreate = new ItemBuilder()
            .HasMenu(menuId)
            .HasIngredients(createRequest.Ingredients)
            .HasCategories(createRequest.Categories)
            .HasGeneralData(createRequest.Name, createRequest.Description, createRequest.Price)
            .Build();

        var createdItem = await _itemRepository.CreateAsync(itemToCreate);

        return Result.Success<ItemsIdDto>(new(menuId, createdItem.Id));
    }

    public async Task<Result> DeleteItemAsync(Guid menuId, Guid itemId)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            return Result.NotFound();

        var orders = await _publisher.Send(new GetActiveOrdersOfItem(itemId));
        if (orders.Any())
            return Result.ValidationError(ItemErrorCodes.ITEM_HAS_PENDING_ORDERS);

        item.MarkAsDeleted();

        await _itemRepository.UpdateAsync(item);
        return Result.Success();
    }

    //TODO: Transition to query?
    public async Task<Result<ItemInfoDto>> GetItemByIdAsync(Guid menuId, Guid itemId, string requesterId)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            return Result.NotFound();

        var creator = await _publisher.Send(new GetIdentityForMenuId(item.MenuId, requesterId));

        return creator == default ? Result.NotFound() : Result.Success(_mapper.Map<ItemInfoDto>(item));
    }

    public async Task<Result<short>> GetCustomerRatingForItemAsync(Guid menuId, Guid itemId, string customerId)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            return Result.NotFound();

        var customerRating = item.Ratings.SingleOrDefault(r => r.CustomerId == customerId);

        return Result.Success(customerRating?.Rating ?? 0);
    }

    public async Task<Result> RateItemAsync(Guid menuId, Guid itemId, string customerId, short rating)
    {
        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            return Result.NotFound();

        item.AddOrChangeRating(customerId, rating);
        await _itemRepository.UpdateAsync(item);

        return Result.Success();
    }

    public async Task<Result> UpdateItemAsync(Guid menuId, Guid itemId, UpdateItemDto updateRequest)
    {
        if (await _validationProvider.ValidateModelAsync(updateRequest) is var valRes && !valRes.IsSuccess)
            return Result.From<Guid>(valRes);

        var item = await _itemRepository.GetByMenuAndIdAsync(menuId, itemId);
        if (item == default)
            return Result.NotFound();

        item.UpdateAllCategories(updateRequest.Categories);
        item.UpdateAllIngredients(updateRequest.Ingredients);
        item.EditGeneralData(updateRequest.Name, updateRequest.Description, updateRequest.Price);

        await _itemRepository.UpdateAsync(item);

        return Result.Success();
    }
}
