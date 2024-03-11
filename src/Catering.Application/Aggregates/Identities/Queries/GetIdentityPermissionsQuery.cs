using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;
using Catering.Application.Results;
using Catering.Domain.Aggregates.Identity;
using MediatR;

namespace Catering.Application.Aggregates.Identities.Queries;

public record GetIdentityPermissionsQuery(string IdentityId) : IRequest<Result<IdentityPermissionsDto>>;

public class GetIdentityHandler(IIdentityRepository<Identity> repository) 
    : IRequestHandler<GetIdentityPermissionsQuery, Result<IdentityPermissionsDto>>
{
    private readonly IIdentityRepository<Identity> _repository = repository;

    public async Task<Result<IdentityPermissionsDto>> Handle(
        GetIdentityPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var identity = await _repository.GetByIdAsync(request.IdentityId);

        if (identity is null)
        {
            return Results.Result.NotFound();
        }

        var permissions = Permissions.GetIdentityPermissions(identity.Role);

        return Results.Result.Success(new IdentityPermissionsDto
        {
            Id = identity.Id,
            Email = identity.Email,
            Role = identity.Role,
            FullName = identity.FullName,
            Permissions = permissions
        });
    }
}
