using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Items.Requests;

internal class GetIdentityWithMenuId : IRequest<Identity>
{
    public Guid MenuId { get; set; }
    public string IdentityId { get; set; }
}
