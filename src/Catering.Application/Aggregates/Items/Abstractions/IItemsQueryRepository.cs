using Catering.Application.Aggregates.Items.Dtos;

namespace Catering.Application.Aggregates.Items.Abstractions;

public interface IItemsQueryRepository
{
    Task<PageBase<ItemInfoDto>> GetPageAsync(ItemsFilter filters);
    Task<List<ItemsLeaderboardDto>> GetMostOrderedFromTheMenuAsync(Guid menuId, int top);
}
