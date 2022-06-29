using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

internal class GetIdentityForMenuId : IRequest<Identity>
{
    public Guid MenuId { get; set; }
    public string IdentityId { get; set; }
}
