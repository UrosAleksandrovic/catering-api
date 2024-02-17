using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

internal class GetIdentityForMenuId : IRequest<Identity>
{
    public Guid MenuId { get; set; }
    public string IdentityId { get; set; }
}
