using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Menus.Requests;
using Catering.Domain.Aggregates.Identity;
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
