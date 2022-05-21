using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Domain.Entities.IdentityAggregate;
using MediatR;

namespace Catering.Application.Aggregates.Menus.Handlers;

internal class GetIdentityByIdHandler : IRequestHandler<GetIdentityById, Identity>
{
    private readonly IIdentityRepository<Identity> _identityRepository;

    public GetIdentityByIdHandler(IIdentityRepository<Identity> identityRepository)
    {
        _identityRepository = identityRepository;
    }

    public Task<Identity> Handle(GetIdentityById request, CancellationToken cancellationToken)
    {
        return _identityRepository.GetByIdAsync(request.Id);
    }
}
