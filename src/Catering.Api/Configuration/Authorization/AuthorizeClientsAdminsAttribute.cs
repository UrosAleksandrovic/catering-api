using Catering.Domain.Aggregates.Identity;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeClientsAdminsAttribute : CateringAuthorizationAttribute
{
    public AuthorizeClientsAdminsAttribute()
        : base(new[] {
            IdentityRole.Super | IdentityRole.Administrator,
            IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee}) 
    { }
}
