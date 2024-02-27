using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Menus.Dtos;
using Catering.Application.Aggregates.Menus.Requests;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Handlers;

internal class GetIdentityInfoForMenuContactHandler 
    : IRequestHandler<GetIdentityInfoForMenuContactRequest, MenuContactDetailedInfoDto>
{
    private readonly ICateringIdentitiesRepository _identitiesRepository;

    public GetIdentityInfoForMenuContactHandler(ICateringIdentitiesRepository identitiesRepository)
    {
        _identitiesRepository = identitiesRepository;
    }

    public async Task<MenuContactDetailedInfoDto> Handle(
        GetIdentityInfoForMenuContactRequest request,
        CancellationToken cancellationToken)
    {
        var identity = await _identitiesRepository.GetByIdAsync(request.IdentityId);

        if (identity == null)
            return null;

        return new MenuContactDetailedInfoDto()
        {
            Email = identity.Email,
            FullName = identity.FullName,
            MenuId = request.MenuId,
            MenuName = request.MenuName,
            IdentityId = identity.Id
        };
    }
}
