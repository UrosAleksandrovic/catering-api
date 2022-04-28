using Catering.Infrastructure.Data;
using Catering.Infrastructure.Mailing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.Infrastructure.DependencyInjection;

public static class PersistanceExtensions
{
    public static IServiceCollection AddCateringPersistance(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(CateringDataSettings.Position);
        services.AddOptions<CateringDataSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContextFactory<CateringDbContext>(options =>
        {
            options.UseNpgsql(settingsSection.Get<CateringDataSettings>().ConnectionString);
        });

        return services;
    }

    public static IServiceCollection AddMailingPersistance(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(MailingDataSettings.Position);
        services.AddOptions<CateringDataSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContextFactory<MailingDbContext>(options =>
        {
            options.UseNpgsql(settingsSection.Get<MailingDataSettings>().ConnectionString);
        });

        return services;
    }
}
