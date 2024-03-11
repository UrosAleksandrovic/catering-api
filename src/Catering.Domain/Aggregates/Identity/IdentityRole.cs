using System.Text;

namespace Catering.Domain.Aggregates.Identity;

public enum IdentityRole
{
    SuperAdmin,
    ClientAdmin,
    ClientEmployee,
    RestaurantAdmin,
    RestaurantEmployee
}

public static class IdentityRoleExtensions
{
    public static bool IsAdministrator(this IdentityRole role)
        => role == IdentityRole.ClientAdmin || role == IdentityRole.RestaurantAdmin || role == IdentityRole.SuperAdmin;

    public static bool IsClientEmployee(this IdentityRole role)
        => role == IdentityRole.ClientAdmin || role == IdentityRole.ClientEmployee;

    public static bool IsRestaurantEmployee(this IdentityRole role)
        => role == IdentityRole.RestaurantAdmin || role == IdentityRole.RestaurantEmployee;

    public static bool IsSuperAdmin(this IdentityRole role)
        => role == IdentityRole.SuperAdmin;
}
