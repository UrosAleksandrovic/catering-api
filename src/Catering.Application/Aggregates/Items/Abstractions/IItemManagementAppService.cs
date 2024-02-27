using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Results;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemManagementAppService
{
    Task<Result<Guid>> CreateItemAsync(Guid menuId, CreateItemDto createRequest);
    Task<Result> UpdateItemAsync(Guid menuId, Guid itemId, UpdateItemDto updateRequest);
    Task<Result> RateItemAsync(Guid menuId, Guid itemId, string customerId, short rating);
    Task<Result> DeleteItemAsync(Guid menuId, Guid itemId);
    Task<Result<short>> GetCustomerRatingForItemAsync(Guid menuId, Guid itemId, string customerId);
    Task<Result<ItemInfoDto>> GetItemByIdAsync(Guid menuId, Guid itemId, string requesterId);
}
