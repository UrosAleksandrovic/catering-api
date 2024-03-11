using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Aggregates.Items.Requests;
using Catering.Application.Filtering;
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
        var result = FilterResult<ItemInfoDto>.Empty<ItemInfoDto>(
            request.Filters.PageSize,
            request.Filters.PageIndex);

        var identityRequest = new GetIdentityForMenuId(request.Filters.MenuId, request.RequesterId);
        var requester = await _publisher.Send(identityRequest, cancellationToken);
        if (requester == default)
            return result;

        var pageBase = await _itemsQueryRepository.GetPageAsync(request.Filters);
        result.TotalNumberOfElements = pageBase.TotalCount;
        result.Value = pageBase.Data;

        return result;
    }
}
