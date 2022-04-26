using Catering.Infrastructure.Data;
using Catering.Infrastructure.Mailing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Catering.Infrastructure.DependencyInjection;

public static class PersistanceExtensions
{
    public static IServiceCollection AddCateringPersistance(
        this IServiceCollection services,
        IOptions<CateringDataSettings> configuration)
    {
        services.AddDbContextFactory<CateringDbContext>(options =>
        {
            options.UseNpgsql(configuration.Value.ConnectionString);
        });

        return services;
    }

    public static IServiceCollection AddMailingPersistance(
        this IServiceCollection services,
        IOptions<MailingDataSettings> configuration)
    {
        services.AddDbContextFactory<MailingDbContext>(options =>
        {
            options.UseNpgsql(configuration.Value.ConnectionString);
        });

        return services;
    }
}
