﻿using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Dtos.Menu;
using Catering.Application.Filtering;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Queries;

public record GetFilteredMenusQuery(MenusFilter Filters) : IRequest<FilterResult<MenuInfoDto>>;

internal class GetFilteredMenusQueryHandler(IMenusQueryRepository queryRepository)
    : IRequestHandler<GetFilteredMenusQuery, FilterResult<MenuInfoDto>>
{
    private readonly IMenusQueryRepository _queryRepository = queryRepository;

    public async Task<FilterResult<MenuInfoDto>> Handle(
        GetFilteredMenusQuery request,
        CancellationToken cancellationToken)
    {
        var result = FilterResult<MenuInfoDto>.Empty<MenuInfoDto>(
            request.Filters.PageSize,
            request.Filters.PageIndex);

        var page = await _queryRepository.GetPageAsync(request.Filters);
        result.TotalNumberOfElements = page.TotalCount;
        result.Value = page.Data;

        return result;
    }
}
