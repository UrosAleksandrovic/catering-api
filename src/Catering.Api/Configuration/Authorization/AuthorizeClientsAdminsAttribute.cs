using Catering.Domain.Aggregates.Identity;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeClientsAdminsAttribute : CateringAuthorizationAttribute
{
    public AuthorizeClientsAdminsAttribute() : base(IdentityRole.ClientAdmin) 
    { }
}
