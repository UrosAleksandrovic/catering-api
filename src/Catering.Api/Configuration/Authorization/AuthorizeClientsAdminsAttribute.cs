using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeClientsAdminsAttribute : CateringAuthorizationAttribute
{
    public AuthorizeClientsAdminsAttribute()
        : base(new[] {
            IdentityRole.Super | IdentityRole.Administrator,
            IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee}) 
    { }
}
