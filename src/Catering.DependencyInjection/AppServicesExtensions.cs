using Catering.Application.Aggregates.Carts;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Expenses;
using Catering.Application.Aggregates.Expenses.Abstractions;
using Catering.Application.Aggregates.Identities;
using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos.Validators;
using Catering.Application.Security.Handlers;
using Catering.Domain.Services;
using Catering.Domain.Services.Abstractions;
using Catering.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IItemManagementAppService, ItemManagementAppService>();
        services.AddScoped<ICartManagementAppService, CartManagementAppService>();
        services.AddScoped<ICustomerManagementAppService, CustomerManagementAppService>();
        services.AddScoped<ICateringIdentitiesManagementAppService, CateringIdentitiesManagementAppService>();
        services.AddScoped<IMenuManagementAppService, MenuManagementAppService>();
        services.AddScoped<IOrderManagementAppService, OrderManagementAppService>();
        services.AddScoped<IExpensesManagementAppService, ExpensesManagementAppService>();
        services.AddScoped<ICustomerReportsAppService, CustomerReportsAppService>();

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
            cfg.RegisterServicesFromAssemblyContaining<LoginLdapIdentity>();
        });

        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();

        services.AddFluentValidationAutoValidation().AddFluentValidationAutoValidation(cfg =>
        {
            cfg.DisableDataAnnotationsValidation = true;
        });

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderingService, OrderingService>();

        return services;
    }
}
