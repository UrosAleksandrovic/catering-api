using Catering.Application.Aggregates.Menus.Dtos;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Requests;

internal class GetIdentityInfoForMenuContactRequest : IRequest<MenuContactDetailedInfoDto>
{
    public Guid MenuId { get; set; }
    public string MenuName { get; set; }
    public string IdentityId { get; set; }
}
