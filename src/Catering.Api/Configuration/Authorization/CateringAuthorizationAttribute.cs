using Catering.Domain.Aggregates.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Catering.Api.Configuration.Authorization;

internal class CateringAuthorizationAttribute : AuthorizeAttribute
{
    public CateringAuthorizationAttribute(params IdentityRole[] identityRoles)
    {
        var finalListOfRoles = identityRoles.Concat([IdentityRole.SuperAdmin]);
        Roles = string.Join(',', finalListOfRoles.Select(r => r.ToString()));
    }
}
