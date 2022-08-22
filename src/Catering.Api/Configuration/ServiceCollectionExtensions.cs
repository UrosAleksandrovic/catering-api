using Catering.Api.Configuration.ErrorHandling;
using Catering.Api.Configuration.ErrorHandling.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catering.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCateringSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.DescribeAllParametersInCamelCase();
            options.SwaggerDoc(OpenApiConstants.Version, new OpenApiInfo
            {
                Title = OpenApiConstants.Title,
                Description = OpenApiConstants.Description,
                Version = OpenApiConstants.Version,
                Contact = new OpenApiContact
                {
                    Email = string.Empty,
                    Name = OpenApiConstants.ContactName,
                    Url = new Uri(OpenApiConstants.ContactUrl)
                }
            });

            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "JWT Authorization with Bearer token in the header",

                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, openApiSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    openApiSecurityScheme,
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddCateringExceptionHandlling(this IServiceCollection services, Assembly assemblyContiningHandlers)
    {
        var types = assemblyContiningHandlers.GetTypes().Where(t => typeof(ErrorHttpResolver<>).IsAssignableFrom(t));

        var publisher = new ErrorPublisher();
        var success = publisher.TryAddExceptionResolvers(types);

        if (!success)
            throw new ArgumentException($"Could not add handlers from assembly: {assemblyContiningHandlers.FullName}");

        services.AddSingleton<IErrorPublisher>(publisher);

        return services;
    }

    public static IServiceCollection AddCateringCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsUrls = configuration.GetSection("CORS").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsUrls)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        return services;
    }
}
