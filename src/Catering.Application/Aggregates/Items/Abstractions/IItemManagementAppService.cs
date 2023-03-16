using Catering.Application.Aggregates.Items.Dtos;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemManagementAppService
{
    Task<Guid> CreateItemAsync(Guid menuId, CreateItemDto createRequest);
    Task UpdateItemAsync(Guid menuId, Guid itemId, UpdateItemDto updateRequest);
    Task RateItemAsync(Guid menuId, Guid itemId, string customerId, short rating);
    Task DeleteItemAsync(Guid menuId, Guid itemId);
    Task<FilterResult<ItemInfoDto>> GetFilteredAsync(ItemsFilter itemFilters, string requesterId);
    Task<short> GetCustomerRatingForItemAsync(Guid menuId, Guid itemId, string customerId);
    Task<ItemInfoDto> GetItemByIdAsync(Guid menuId, Guid itemId, string requesterId);
    Task<List<ItemsLeaderboardDto>> GetMostOrderedFromTheMenuAsync(int top, Guid menuId);
}
