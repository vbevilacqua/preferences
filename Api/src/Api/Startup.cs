using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Auth0;
using Api.StartupServices;
using Api.Swagger;
using Application;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using TanvirArjel.EFCore.GenericRepository;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InitServices(services);
            InitContext(services);
            InitAuth0(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var scope = app.ApplicationServices.CreateScope();

            app
                .UseSwaggerFeatures(Configuration, env)
                .UseAuthentication()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                    endpoints.MapFallback(() => Results.Redirect("/swagger"));
                });
        }

        public static void InitServices(IServiceCollection services)
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
            services.AddGenericRepository<PreferencesDbContext>();
            services.AddQueryRepository<PreferencesDbContext>();
        }

        public void InitAuth0(IServiceCollection services)
        {
            services.AddMvcCore(options => { options.AddBaseAuthorizationFilters(Configuration); }).AddApiExplorer();

            // JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = Configuration.GetValue<string>("Authentication:ApiName");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
            
            var domain = Configuration.GetValue<string>("Authentication:Authority"); 
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
                
        });
        }

        public static void InitContext(IServiceCollection services)
        {
            services.AddDbContext<PreferencesDbContext>();
        }
    }
}