using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

public record GetIdentityForMenuId(Guid MenuId, string IdentityId) : IRequest<Identity>;
