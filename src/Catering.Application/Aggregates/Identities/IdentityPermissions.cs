using Catering.Domain.Entities.IdentityAggregate;

namespace Catering.Application.Aggregates.Identities;

[Flags]
public enum IdentityPermissions
{
    ListMenus = 1,
    EditMenus = 2,
    DeleteMenus = 4,
    AddMenuItems = 8,
    DeleteMenuItems = 16,
    ListOrders = 32,
    ChangeOrderStatus = 64,
    ChangeGlobalSettings = 128,
    ListUsers = 256,
    InviteUsers = 512,
    AddUsers = 1024,
    DeleteUsers = 2048,
    ListReports = 4096,
    GenerateReports = 8192,
    NoPermissions = 16384,
}

public static class IdentityPermissionsExtensions
{
    public static IdentityPermissions GetFromRole(this IdentityRole role)
    {
        if (role.IsSuperAdmin())
            return GetSuperAdminPermissions();

        if (role.IsClientEmployee() && role.IsAdministrator())
            return GetClientAdminPermissions();

        if (role.IsClientEmployee())
            return GetClientEmployeesPermissions();

        if (role.IsRestaurantEmployee())
            return GetRestaurantEmployeePermissions();

        return IdentityPermissions.NoPermissions;
    } 

    public static IdentityPermissions GetSuperAdminPermissions()
    {
        var permissions = IdentityPermissions.ListMenus;
        foreach (var identityPermission in Enum.GetValues<IdentityPermissions>())
            permissions |= identityPermission;

        return permissions;
    }

    public static IdentityPermissions GetClientEmployeesPermissions()
        => IdentityPermissions.ListMenus;

    public static IdentityPermissions GetRestaurantEmployeePermissions()
        => IdentityPermissions.ChangeOrderStatus;

    public static IdentityPermissions GetClientAdminPermissions()
        => GetSuperAdminPermissions();
}
