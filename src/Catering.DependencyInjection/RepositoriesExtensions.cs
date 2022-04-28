using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Mailing.Emails;
using Catering.Infrastructure.Data.Repositories;
using Catering.Infrastructure.Mailing.Emails;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.Infrastructure.DependencyInjection;

public static class RepositoriesExtensions
{
    public static IServiceCollection AddCateringRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IExternalIdentityRepository, ExternalIdentityRepository>();

        return services;
    }

    public static IServiceCollection AddMailingRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEmailRepository, EmailRepository>();

        return services;
    }
}
