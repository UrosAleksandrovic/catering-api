﻿using System.Text;
using Catering.Application.Security;
using Catering.Domain.Aggregates.Identity;
using Catering.Infrastructure.Security;
using Catering.Infrastructure.Security.Ldap;
using Catering.Infrastructure.Security.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Catering.DependencyInjection;

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
        services.AddScoped<ITokenAtuhenticator<Identity>, LdapTokenAuthenticator>();
        services.AddScoped<ITokenAtuhenticator<CateringIdentity>, CateringTokenAuthenticator>();

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

        settingsSection = configuration.GetSection(SecurityAesSettings.Position);
        services.AddOptions<SecurityAesSettings>()
            .Bind(settingsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IDataProtector, DataProtector>();

        return services;
    }

    public static IServiceCollection AddCateringAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
            {
                var jwtSettings = configuration.GetSection(SecurityJwtSettings.Position).Get<SecurityJwtSettings>();
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key))
                };
            });

        return services;
    }
}
