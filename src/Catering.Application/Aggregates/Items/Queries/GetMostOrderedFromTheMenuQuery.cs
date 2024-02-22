using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using MediatR;

namespace Catering.Application.Aggregates.Items.Queries;

public record GetMostOrderedFromTheMenuQuery(Guid MenuId, int Top) : IRequest<List<ItemsLeaderboardDto>>;

internal class GetMostOrderedFromTheMenuQueryHandler
    : IRequestHandler<GetMostOrderedFromTheMenuQuery, List<ItemsLeaderboardDto>>
{
    private readonly IItemsQueryRepository _itemRepository;

    public GetMostOrderedFromTheMenuQueryHandler(IItemsQueryRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public Task<List<ItemsLeaderboardDto>> Handle(
        GetMostOrderedFromTheMenuQuery request,
        CancellationToken cancellationToken)
        => _itemRepository.GetMostOrderedFromTheMenuAsync(request.MenuId, request.Top);
}
