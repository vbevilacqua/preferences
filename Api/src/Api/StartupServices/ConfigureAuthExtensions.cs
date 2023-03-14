using System.Security.Claims;
using Infrastructure.Auth0;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.StartupServices
{
    public static class ConfigureAuthExtensions
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvcCore(options => { options.AddBaseAuthorizationFilters(configuration); }).AddApiExplorer();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = configuration.GetValue<string>("Authentication:ApiName");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            var domain = configuration.GetValue<string>("Authentication:Authority");
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:solution", policy => policy.Requirements.Add(new
                    HasScopeRequirement("read:solution", domain)));
                options.AddPolicy("write:solution", policy => policy.Requirements.Add(new
                    HasScopeRequirement("write:solution", domain)));
                options.AddPolicy("read:user", policy => policy.Requirements.Add(new
                    HasScopeRequirement("read:user", domain)));
                options.AddPolicy("write:user", policy => policy.Requirements.Add(new
                    HasScopeRequirement("write:user", domain)));
                options.AddPolicy("read:preference", policy => policy.Requirements.Add(new
                    HasScopeRequirement("read:preference", domain)));
                options.AddPolicy("write:preference", policy => policy.Requirements.Add(new
                    HasScopeRequirement("write:preference", domain)));

            });
            return services;
        }
    }
}

