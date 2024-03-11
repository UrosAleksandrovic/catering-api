using Catering.Domain.Aggregates.Identity;

namespace Catering.Api.Configuration.Authorization;

internal class AuthorizeRestaurantEmployee : CateringAuthorizationAttribute
{
    public AuthorizeRestaurantEmployee() : base(IdentityRole.RestaurantAdmin, IdentityRole.RestaurantEmployee)
    { }
}
