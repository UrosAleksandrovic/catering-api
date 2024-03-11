using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Requests;

internal class GetIdentityWithId : IRequest<Identity>
{
    public string IdentityId { get; set; }
}
