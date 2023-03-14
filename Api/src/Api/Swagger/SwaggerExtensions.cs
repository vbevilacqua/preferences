using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger;
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerFeatures(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder UseSwaggerFeatures(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            return app;
        }

        var clientId = config.GetValue<string>("Authentication:SwaggerClientId");
        app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1.0");

                options.DocumentTitle = "Preferences Documentation";
                options.OAuthClientId(clientId);
                options.OAuthAppName("Preferences");
                options.OAuthUsePkce();
                options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "audience", config.GetValue<string>("Authentication:ApiName") }
                });
            });

        return app;
    }
}
