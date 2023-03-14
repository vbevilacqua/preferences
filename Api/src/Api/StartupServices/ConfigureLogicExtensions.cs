namespace Api.StartupServices
{
    using Api.Swagger;
    using Application;
    using Domain.Common;
    using Infrastructure.Auth0;
    using Infrastructure.Settings;
    using Microsoft.AspNetCore.Authorization;

    public static class ConfigureLogicExtensions
    {
        public static IServiceCollection ConfigureLogic(this IServiceCollection services)
        {
            // Register all mediator dependencies.
            services.AddApplication();
            services.AddHealthChecks();
            services.AddCustomApiVersioning()
                    .AddSwaggerFeatures()
                    .AddHttpContextAccessor();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddHttpClient();
            services.AddSingleton<ISettings, UserSettings>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            return services;
        }
    }
}
