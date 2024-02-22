using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Requests;
using MediatR;

namespace Catering.Application.Aggregates.Items.Queries;

public record GetFilteredItemsQuery(ItemsFilter Filters, string RequesterId) : IRequest<FilterResult<ItemInfoDto>>;

internal class GetFilteredItemsQueryHandler(IItemsQueryRepository itemsQueryRepository, IMediator publisher)
    : IRequestHandler<GetFilteredItemsQuery, FilterResult<ItemInfoDto>>
{
    private readonly IItemsQueryRepository _itemsQueryRepository = itemsQueryRepository;
    private readonly IMediator _publisher = publisher;

    public async Task<FilterResult<ItemInfoDto>> Handle(
        GetFilteredItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = FilterResult<ItemInfoDto>.GetEmpty<ItemInfoDto>(
            request.Filters.PageSize,
            request.Filters.PageIndex);

        var identityRequest = new GetIdentityForMenuId
        {
            IdentityId = request.RequesterId,
            MenuId = request.Filters.MenuId
        };
        var requester = await _publisher.Send(identityRequest, cancellationToken);
        if (requester == default)
            return result;

        var pageBase = await _itemsQueryRepository.GetPageAsync(request.Filters);
        result.TotalNumberOfElements = pageBase.TotalCount;
        result.Result = pageBase.Data;

        return result;
    }
}
