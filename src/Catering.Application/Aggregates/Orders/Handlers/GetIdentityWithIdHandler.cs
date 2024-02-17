using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class GetIdentityWithIdHandler : IRequestHandler<GetIdentityWithId, Identity>
{
    private readonly IIdentityRepository<Identity> _repository;

    public GetIdentityWithIdHandler(IIdentityRepository<Identity> repository)
    {
        _repository = repository;
    }

    public async Task<Identity> Handle(GetIdentityWithId request, CancellationToken cancellationToken)
        => await _repository.GetByIdAsync(request.IdentityId);
}
