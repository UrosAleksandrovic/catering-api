using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Mailing.Emails;
using Catering.Domain.Aggregates.Identity;
using Catering.Infrastructure.Data.Repositories;
using Catering.Infrastructure.Mailing.Emails;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class RepositoriesExtensions
{
    public static IServiceCollection AddCateringRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddTransient<IItemsQueryRepository, ItemsQueryRepository>();

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartQueryRepository, CartQueryRepository>();

        services.AddScoped<IMenusRepository, MenusRepository>();
        services.AddTransient<IMenusQueryRepository, MenusQueryRepository>();

        services.AddScoped<IOrderRepository, OrdersRepository>();
        services.AddTransient<IOrdersQueryRepository, OrdersQueryRepository>();

        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddTransient<IExpensesQueryRepository, ExpensesQueryRepository>();

        services.AddTransient<ICustomerReportsRepository, CustomerReportsRepository>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddTransient<ICustomerQueryRepository, CustomerQueryRepository>();

        services.AddTransient<ICateringIdentitiesRepository, CateringIdentityRepository>();

        services.AddScoped<IIdentityRepository<Identity>, IdentityRepository<Identity>>();
        services.AddTransient<IIdentityQueryRepository<Identity>, IdentityQueryRepository<Identity>>();

        return services;
    }

    public static IServiceCollection AddMailingRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEmailRepository, EmailRepository>();

        return services;
    }
}
