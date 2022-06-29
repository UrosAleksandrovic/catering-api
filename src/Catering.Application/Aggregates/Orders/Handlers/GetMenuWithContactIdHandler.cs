using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Requests;
using MediatR;

namespace Catering.Application.Aggregates.Orders.Handlers;

internal class GetMenuWithContactIdHandler : IRequestHandler<GetMenuWithContactId, Guid?>
{
    private readonly IMenuRepository _repository;

    public GetMenuWithContactIdHandler(IMenuRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid?> Handle(GetMenuWithContactId request, CancellationToken cancellationToken)
        => (await _repository.GetByContactIdAsync(request.ContactId))?.Id;
}
