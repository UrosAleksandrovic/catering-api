using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Requests;

internal class GetIdentityByIdRequest : IRequest<Identity>
{
    public string Id { get; set; }
}
