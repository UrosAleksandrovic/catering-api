﻿using Catering.Application.Aggregates.Items.Dtos;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemManagementAppService
{
    public Task<Guid> CreateItemAsync(CreateItemDto createRequest);
    public Task UpdateItemAsync(Guid itemId, UpdateItemDto updateRequest);
    public Task RateItemAsync(Guid itemId, string customerId, short rating);
    public Task DeleteItemAsync(Guid itemId);
    public Task<FilterResult<ItemInfoDto>> GetFilteredAsync(ItemsFilter itemFilters, string requestorId);
    public Task<short> GetCustomerRatingForItemAsync(Guid itemId, string customerId);
    public Task<ItemInfoDto> GetItemByIdAsync(Guid itemId, string requestorId);
    public Task<List<ItemsLeaderboardDto>> GetMostOrderedFromTheMenuAsync(int top, Guid menuId);
}
