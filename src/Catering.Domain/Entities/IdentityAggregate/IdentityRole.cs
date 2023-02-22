using System.Text;

namespace Catering.Domain.Entities.IdentityAggregate;

[Flags]
public enum IdentityRole
{
    Super = 1,
    Administrator = 2,
    Client = 4,
    Restaurant = 8,
    Employee = 16,
}

public static class IdentityRoleExtensions
{
    public static bool IsAdministrator(this IdentityRole role)
        => role.HasFlag(IdentityRole.Administrator);

    public static bool IsClientEmployee(this IdentityRole role)
        => role.HasFlag(IdentityRole.Client) && role.HasFlag(IdentityRole.Employee);

    public static bool IsRestaurantEmployee(this IdentityRole role)
        => role.HasFlag(IdentityRole.Restaurant) && role.HasFlag(IdentityRole.Employee);

    public static bool IsSuperAdmin(this IdentityRole role)
        => role.HasFlag(IdentityRole.Super) && role.HasFlag(IdentityRole.Administrator);

    public static IEnumerable<IdentityRole> GetRoles(this IdentityRole input)
        => Enum.GetValues<IdentityRole>().Where(value => input.HasFlag(value));

    public static IdentityRole GetClientAdministrator() => IdentityRole.Administrator | IdentityRole.Client | IdentityRole.Employee;

    public static IdentityRole GetSuperAdministrator() => IdentityRole.Super | IdentityRole.Administrator;

    public static IdentityRole GetRestaurantEmployee() => IdentityRole.Restaurant | IdentityRole.Employee;
    public static IdentityRole GetClientEmployee() => IdentityRole.Client | IdentityRole.Employee;

    public static string ToIdentityString(this IdentityRole input)
    {
        var identityRole = new StringBuilder();
        foreach (var role in input.GetRoles())
            identityRole.Append(role.ToString());

        return identityRole.ToString();
    }
}
