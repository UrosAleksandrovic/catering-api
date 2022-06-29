using Microsoft.AspNetCore.Authorization;

namespace Catering.Api.Configuration.Authorization;

internal class CateringAuthorizationAttribute : AuthorizeAttribute
{
    public CateringAuthorizationAttribute(params string[] identityRoles)
    {
        Roles = string.Join(',', identityRoles);
    }
}
