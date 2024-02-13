using Serilog;

namespace Catering.Api.Configuration;

public static class ConfiurationHostBuilderExtensions
{
    public static void AddCateringLogging(this ConfigureHostBuilder builder)
    {
        builder.UseSerilog((ctx, config) =>
        {
            if (ctx.HostingEnvironment.IsDevelopment())
                config.MinimumLevel.Debug();

            config.WriteTo.Console();
        });
    }
}
