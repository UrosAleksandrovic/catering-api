using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders;
using Microsoft.Extensions.DependencyInjection;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Identites;
using Catering.Application.Aggregates.Carts;
using Catering.Application.Aggregates.Items;
using Catering.Infrastructure;
using MediatR;
using Catering.Application.Aggregates.Orders.Handlers;

namespace Catering.DependencyInjection;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IItemManagementAppService, ItemManagementAppService>();
        services.AddScoped<ICartManagementAppService, CartManagementAppService>();
        services.AddScoped<ICustomerManagementAppService, CustomerManagementAppService>();
        services.AddScoped<IMenuManagementAppService, MenuManagementAppService>();
        services.AddScoped<IOrderManagementAppService, OrderManagementAppService>();

        return services;
    }

    public static IServiceCollection AddCateringMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CateringAggregatesMapperConfiguration));

        return services;
    }

    public static IServiceCollection AddCateringMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.AsSingleton();
        }, typeof(OrderPlacedHandler).Assembly);

        return services;
    }
}
