using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Filtering;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Queries;

public record GetFilteredMenuContactsQuery(MenusFilter Filters) : IRequest<FilterResult<MenuContactDetailedInfoDto>>;

internal class GetMenuContactsQueryHandler(IMenusQueryRepository queryRepository) :
    IRequestHandler<GetFilteredMenuContactsQuery, FilterResult<MenuContactDetailedInfoDto>>
{
    private readonly IMenusQueryRepository _queryRepository = queryRepository;

    public async Task<FilterResult<MenuContactDetailedInfoDto>> Handle(
        GetFilteredMenuContactsQuery request,
        CancellationToken cancellationToken)
    {
        var result = FilterResult<MenuContactDetailedInfoDto>.Empty<MenuContactDetailedInfoDto>(
            request.Filters.PageIndex,
            request.Filters.PageSize);

        var page = await _queryRepository.GetContactsAsync(request.Filters);
        result.TotalNumberOfElements = page.TotalCount;
        result.Value = page.Data;

        return result;
    }
}
