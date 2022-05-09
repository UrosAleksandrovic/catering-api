using Catering.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class CateringDependencyExtensions
{
    public static IServiceCollection AddCateringDependecies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCateringPersistance(configuration);
        services.AddMailingPersistance(configuration);

        services.AddCateringRepositories();
        services.AddMailingRepositories();

        services.AddAuthenticators(configuration);
        services.AddDataProtection(configuration);

        services.AddEmailSending(configuration);

        services.AddAppServices();
        services.AddCateringMapper();
        services.AddCateringMediator();
        services.AddFluentValidation();
        services.AddDomainServices();

        return services;
    }
}
