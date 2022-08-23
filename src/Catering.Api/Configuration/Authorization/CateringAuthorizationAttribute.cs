using Catering.Domain.Entities.IdentityAggregate;
using Microsoft.AspNetCore.Authorization;

namespace Catering.Api.Configuration.Authorization;

internal class CateringAuthorizationAttribute : AuthorizeAttribute
{
    public CateringAuthorizationAttribute(params IdentityRole[] identityRoles)
    {
        Roles = string.Join(',', identityRoles.Select(r => r.ToIdentityString()));
    }
}
