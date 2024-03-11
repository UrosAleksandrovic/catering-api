using Catering.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class CachingExtensions
{
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(CachingSettings.Position);
        services.AddOptions<CachingSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var settings = settingsSection.Get<CachingSettings>();
        if (settings.IsInMemory)
            services.AddDistributedMemoryCache();

        return services;
    }
}
