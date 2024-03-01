using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Filtering;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Identities.Queries;

public record GetFilteredIdentitiesQuery(IdentityFilter Filters) : IRequest<FilterResult<IdentityInfoDto>>;

public class GetFilteredIdentitiesQueryHandler(IIdentityQueryRepository<Identity> identityRepository)
    : IRequestHandler<GetFilteredIdentitiesQuery, FilterResult<IdentityInfoDto>>
{
    private readonly IIdentityQueryRepository<Identity> _queryRepository = identityRepository;

    public async Task<FilterResult<IdentityInfoDto>> Handle(
        GetFilteredIdentitiesQuery request,
        CancellationToken cancellationToken)
    {
        var identities = await _queryRepository.GetPageAsync<IdentityInfoDto>(request.Filters);

        var primaryResult = FilterResult<IdentityInfoDto>.Empty<IdentityInfoDto>(request.Filters);

        primaryResult.Value = identities.Data;
        primaryResult.TotalNumberOfElements = identities.TotalCount;

        return primaryResult;
    }
}
