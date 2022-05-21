using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeCompanyEmployeeAttribute : CateringAuthorizationAttribute
{
    public AuthorizeCompanyEmployeeAttribute()
        : base(new[] { 
            IdentityRole.SuperAdministrator,
            IdentityRole.CompanyAdministrator,
            IdentityRole.CompanyEmployee })
    { }
}
