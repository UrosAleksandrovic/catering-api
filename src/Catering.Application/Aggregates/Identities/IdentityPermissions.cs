using Catering.Domain.Aggregates.Identity;

namespace Catering.Application.Aggregates.Identities;

public static class Permissions
{
    public const string MENUS_READ = nameof(MENUS_READ);
    public const string MENUS_WRITE = nameof(MENUS_WRITE);
    public const string MENU_ITEMS_READ = nameof(MENU_ITEMS_READ);
    public const string MENU_ITEMS_WRITE = nameof(MENU_ITEMS_WRITE);
    public const string ORDERS_READ = nameof(ORDERS_READ);
    public const string ORDERS_WRITE = nameof(ORDERS_WRITE);
    public const string ORDERS_STATUS_WRITE = nameof(ORDERS_STATUS_WRITE);
    public const string CART_WRITE = nameof(CART_WRITE);
    public const string EXPENSES_READ = nameof(EXPENSES_READ);
    public const string EXPENSES_WRITE = nameof(EXPENSES_WRITE);
    public const string IDENTITIES_WRITE = nameof(IDENTITIES_WRITE);
    public const string REPORTS_READ = nameof(REPORTS_READ);
    public const string REPORTS_WRITE = nameof(REPORTS_WRITE);
    public const string SETTINGS_WRITE = nameof(SETTINGS_WRITE);

    public static string[] GetIdentityPermissions(IdentityRole role)
        => role switch 
        { 
            IdentityRole.RestaurantEmployee => RestaurantEmployeePermissions, 
            IdentityRole.RestaurantAdmin => RestaurantAdminPermissions,
            IdentityRole.ClientEmployee => ClientEmployeePermissions,
            IdentityRole.ClientAdmin => ClientAdministratorPermissions,
            IdentityRole.SuperAdmin => ClientAdministratorPermissions,
            _ => []
        };

    private static string[] RestaurantEmployeePermissions => 
        [
            MENUS_READ,
            MENU_ITEMS_READ,
            ORDERS_READ,
            ORDERS_STATUS_WRITE
        ];

    private static string[] RestaurantAdminPermissions => RestaurantEmployeePermissions;

    private static string[] ClientEmployeePermissions =>
        [
            MENUS_READ,
            MENU_ITEMS_READ,
            ORDERS_WRITE,
            CART_WRITE,
            EXPENSES_READ,
        ];

    private static string[] ClientAdministratorPermissions =>
        [
            MENUS_WRITE,
            MENU_ITEMS_WRITE,
            ORDERS_WRITE,
            ORDERS_STATUS_WRITE,
            EXPENSES_WRITE,
            IDENTITIES_WRITE,
            REPORTS_WRITE,
            SETTINGS_WRITE
        ];
}
