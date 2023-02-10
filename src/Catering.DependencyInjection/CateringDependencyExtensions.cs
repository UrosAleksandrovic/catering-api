using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class CateringDependencyExtensions
{
    public static IServiceCollection AddCateringDependecies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCateringPersistence(configuration);
        services.AddMailingPersistence(configuration);

        services.AddCateringRepositories();
        services.AddMailingRepositories();

        services.AddAuthenticators(configuration);
        services.AddDataProtection(configuration);

        services.AddEmailSending(configuration);

        services.AddAppServices();
        services.AddCateringMapper();
        services.AddFluentValidation();
        services.AddDomainServices();

        services.AddCateringMediator();
        return services;
    }
}
