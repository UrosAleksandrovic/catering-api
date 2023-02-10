using Catering.Application.Aggregates.Items.Dtos;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemManagementAppService
{
    public Task<Guid> CreateItemAsync(Guid menuId, CreateItemDto createRequest);
    public Task UpdateItemAsync(Guid menuId, Guid itemId, UpdateItemDto updateRequest);
    public Task RateItemAsync(Guid menuId, Guid itemId, string customerId, short rating);
    public Task DeleteItemAsync(Guid menuId, Guid itemId);
    public Task<FilterResult<ItemInfoDto>> GetFilteredAsync(ItemsFilter itemFilters, string requesterId);
    public Task<short> GetCustomerRatingForItemAsync(Guid menuId, Guid itemId, string customerId);
    public Task<ItemInfoDto> GetItemByIdAsync(Guid menuId, Guid itemId, string requesterId);
    public Task<List<ItemsLeaderboardDto>> GetMostOrderedFromTheMenuAsync(int top, Guid menuId);
}
