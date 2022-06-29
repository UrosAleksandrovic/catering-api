using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Items.Requests;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Items.Handlers;

internal class GetIdentityWithMenuIdHandler : IRequestHandler<GetIdentityForMenuId, Identity>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IIdentityRepository<Identity> _identity;

    public GetIdentityWithMenuIdHandler(
        IMenuRepository menuRepository,
        IIdentityRepository<Identity> identity)
    {
        _menuRepository = menuRepository;
        _identity = identity;
    }

    public async Task<Identity> Handle(GetIdentityForMenuId request, CancellationToken cancellationToken)
    {
        var identity = await _identity.GetByIdAsync(request.IdentityId);
        if (identity == default)
            return default;

        if (identity.IsAdministrator || identity.IsCompanyEmployee)
            return identity;

        var menu = await _menuRepository.GetByIdAsync(request.MenuId);
        if (menu == default)
            return default;

        if (identity.IsRestourantEmployee)
            return menu.HasContact(identity.Id) ? identity : default;

        return default;
    }
}
