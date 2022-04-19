using Catering.Application.Aggregates.Items.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.Infrastructure.DependencyInjection;

public static class RepositoriesExtensions
{
    public static IServiceCollection AddCateringRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, IItemRepository>()

        return services;
    }
}
