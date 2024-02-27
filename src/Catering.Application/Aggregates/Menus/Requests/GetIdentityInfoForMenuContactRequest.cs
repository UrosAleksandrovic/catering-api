using Catering.Application.Aggregates.Menus.Dtos;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Requests;

internal record GetIdentityInfoForMenuContactRequest(Guid MenuId, string MenuName, string IdentityId) 
    : IRequest<MenuContactDetailedInfoDto>;
