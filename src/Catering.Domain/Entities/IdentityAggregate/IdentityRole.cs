using Ardalis.GuardClauses;

namespace Catering.Domain.Entities.IdentityAggregate;

public static class IdentityRole
{
    public const string SuperAdministrator = "SuperAdmin";

    public const string CompanyAdministrator = "CompanyAdmin";
    public const string CompanyEmployee = "CompanyEmployee";

    public const string RestourantEmployee = "RestourantEmployee";

    public static bool IsAdministratorRole(string roleName)
    {
        Guard.Against.NullOrWhiteSpace(roleName);

        return roleName.Equals(SuperAdministrator) || roleName.Equals(CompanyAdministrator);
    }
}
