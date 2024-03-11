namespace Catering.Api.Configuration;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCateringSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint(
                $"/swagger/{OpenApiConstants.Version}/swagger.json",
                OpenApiConstants.Name);

            config.RoutePrefix = string.Empty;
            config.DocumentTitle = OpenApiConstants.Title;
            config.ConfigObject.PersistAuthorization = true;
        });

        return app;
    }
}
