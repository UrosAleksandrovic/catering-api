using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Requests;

internal class GetIdentityById : IRequest<Identity>
{
    public string Id { get; set; }
}
