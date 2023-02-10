using Catering.Infrastructure.Data;
using Catering.Infrastructure.Mailing;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.DependencyInjection;

public static class PersistenceExtensions
{
    public static IServiceCollection AddCateringPersistence(
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
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IServiceCollection AddMailingPersistence(
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
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IApplicationBuilder ApplyCateringMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CateringDbContext>();
        dbContext.Database.Migrate();

        return app;
    }

    public static IApplicationBuilder ApplyMailingMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MailingDbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
