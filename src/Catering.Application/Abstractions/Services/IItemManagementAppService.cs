using Catering.Application.Dtos;
using Catering.Application.Dtos.Item;
using Catering.Application.Filters;

namespace Catering.Application.Abstractions.Services;

public interface IItemManagementAppService
{
    public Task<Guid> CreateItemAsync(CreateItemDto createRequest);
    public Task<Guid> UpdateItemAsync(Guid itemId, UpdateItemDto updateRequest);
    public Task RateItemAsync(Guid itemId, string userId, short rating);
    public Task DeleteItemAsync(Guid itemId);
    public Task<FilterResult<ItemInfoDto>> GetFilteredAsync(ItemsFilter itemFilters);
    public Task<short> GetUserRatingForItemAsync(Guid itemId, string userId);
    public Task<ItemInfoDto> GetItemByIdAsync(Guid itemId);
}
