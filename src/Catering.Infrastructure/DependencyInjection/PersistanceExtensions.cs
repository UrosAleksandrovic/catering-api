using Catering.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Catering.Infrastructure.DependencyInjection;

public static class PersistanceExtensions
{
    public static IServiceCollection AddCateringPersistance(IServiceCollection services, IOptions<CateringDataOptions> configuration)
    {
        services.AddDbContextFactory<CateringDbContext>(options =>
        {
            options.UseNpgsql(configuration.Value.ConnectionString);
        });

        return services;
    }
}
