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
using Catering.Application.Validation;
using Catering.Domain.Services;
using Catering.Domain.Services.Abstractions;
using Catering.Infrastructure;
using FluentValidation;
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
        services.AddScoped<IMenusManagementAppService, MenuManagementAppService>();
        services.AddScoped<IOrderManagementAppService, OrderManagementAppService>();
        services.AddScoped<IExpensesManagementAppService, ExpensesManagementAppService>();

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
        services.AddScoped<IValidationProvider, ValidationProvider>();
        services.AddScoped<Application.Validation.IValidatorFactory, ValidatorFactory>();
        services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>(includeInternalTypes: true);

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderingService, OrderingService>();

        return services;
    }
}
