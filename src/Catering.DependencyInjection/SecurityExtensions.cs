using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Infrastructure.Security;
using Catering.Infrastructure.Security.Ldap;
using Catering.Infrastructure.Security.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.Infrastructure.DependencyInjection;

public static class SecurityExtensions
{
    public static IServiceCollection AddAuthenticators(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(SecurityJwtSettings.Position);
        services.AddOptions<SecurityJwtSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        settingsSection = configuration.GetSection(SecurityLdapSettings.Position);
        services.AddOptions<SecurityLdapSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ITokenAtuhenticator<Customer>, LdapTokenAuthenticator>();
        services.AddScoped<ITokenAtuhenticator<ExternalIdentity>, CateringTokenAuthenticator>();

        return services;
    }

    public static IServiceCollection AddDataProtection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection(SecurityLdapSettings.Position);
        services.AddOptions<SecurityLdapSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        settingsSection = configuration.GetSection(SecurityShaSettings.Position);
        services.AddOptions<SecurityShaSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IDataProtector, DataProtector>();

        return services;
    }
}
