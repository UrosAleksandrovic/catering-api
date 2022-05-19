using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeCompanyAdminsAttribute : CateringAuthorizationAttribute
{
    public AuthorizeCompanyAdminsAttribute()
        : base(new[] { IdentityRole.SuperAdministrator, IdentityRole.CompanyAdministrator }) { }
}
