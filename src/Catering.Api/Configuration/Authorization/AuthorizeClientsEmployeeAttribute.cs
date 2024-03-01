using Catering.Domain.Aggregates.Identity;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeClientsEmployeeAttribute : CateringAuthorizationAttribute
{
    public AuthorizeClientsEmployeeAttribute() : base(IdentityRole.ClientAdmin, IdentityRole.ClientEmployee)
    { }
}
