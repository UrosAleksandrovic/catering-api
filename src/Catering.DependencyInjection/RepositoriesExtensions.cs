﻿using Catering.Application.Aggregates.Carts.Abstractions;
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
        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<ICartRepository, CartRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IMenuRepository, MenuRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IExpensesRepository, ExpensesRepository>();
        services.AddTransient<ICustomerReportsRepository, CustomerReportsRepository>();
        services.AddTransient<ICateringIdentitiesRepository, CateringIdentityRepository>();
        services.AddTransient<IIdentityRepository<Identity>, IdentityRepository<Identity>>();

        return services;
    }

    public static IServiceCollection AddMailingRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEmailRepository, EmailRepository>();

        return services;
    }
}
