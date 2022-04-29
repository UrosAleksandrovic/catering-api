﻿using Catering.Application.Aggregates.Carts;
using Catering.Application.Aggregates.Carts.Abstractions;
using Catering.Application.Aggregates.Identites;
using Catering.Application.Aggregates.Identites.Abstractions;
using Catering.Application.Aggregates.Items;
using Catering.Application.Aggregates.Items.Abstractions;
using Catering.Application.Aggregates.Menus;
using Catering.Application.Aggregates.Menus.Abstractions;
using Catering.Application.Aggregates.Orders;
using Catering.Application.Aggregates.Orders.Abstractions;
using Catering.Application.Aggregates.Orders.Dtos.Validators;
using Catering.Application.Aggregates.Orders.Handlers;
using Catering.Infrastructure;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddMvc().AddFluentValidation(fv =>
        {
            fv.DisableDataAnnotationsValidation = true;
            fv.RegisterValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
        });

        return services;
    }
}
