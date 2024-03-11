using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Items.Dtos;
using Catering.Application.Results;
using MediatR;

namespace Catering.Application.Aggregates.Items.Queries;

public record GetMostOrderedFromTheMenuQuery(Guid MenuId, int Top) : IRequest<Result<List<ItemsLeaderboardDto>>>;

internal class GetMostOrderedFromTheMenuQueryHandler(IItemsQueryRepository queryRepository)
    : IRequestHandler<GetMostOrderedFromTheMenuQuery, Result<List<ItemsLeaderboardDto>>>
{
    private readonly IItemsQueryRepository _queryRepository = queryRepository;

    public async Task<Result<List<ItemsLeaderboardDto>>> Handle(
        GetMostOrderedFromTheMenuQuery request,
        CancellationToken cancellationToken)
        => Result.Success(await _queryRepository.GetMostOrderedFromTheMenuAsync(request.MenuId, request.Top));
}
