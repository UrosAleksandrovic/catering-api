namespace Catering.Domain.Entities.IdentityAggregate;

[Flags]
public enum IdentityPermissions : byte
{
    CompanyAdministrator = 1,
    CompanyEmployee = 2,
    RestourantEmployee = 4
}
