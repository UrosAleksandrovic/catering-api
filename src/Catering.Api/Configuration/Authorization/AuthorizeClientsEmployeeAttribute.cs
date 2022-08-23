using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeClientsEmployeeAttribute : CateringAuthorizationAttribute
{
    public AuthorizeClientsEmployeeAttribute()
        : base(new[] { 
            IdentityRole.Super | IdentityRole.Administrator,
            IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee,
            IdentityRole.Client | IdentityRole.Employee })
    { }
}
