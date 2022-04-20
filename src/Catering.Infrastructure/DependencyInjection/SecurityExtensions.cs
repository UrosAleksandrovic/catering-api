using Catering.Application.Security;
using Catering.Domain.Entities.IdentityAggregate;
using Catering.Infrastructure.Security;
using Catering.Infrastructure.Security.Ldap;
using Microsoft.Extensions.DependencyInjection;

namespace Catering.Infrastructure.DependencyInjection;

public static class SecurityExtensions
{
    public static IServiceCollection AddAuthenticators(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ITokenAtuhenticator<Customer>, LdapTokenAuthenticator>();
        services.AddScoped<ITokenAtuhenticator<ExternalIdentity>, CateringTokenAuthenticator>();

        return services;
    }

    public static IServiceCollection AddDataProtection(this IServiceCollection services)
    {
        services.AddScoped<IDataProtector, DataProtector>();

        return services;
    }
}
