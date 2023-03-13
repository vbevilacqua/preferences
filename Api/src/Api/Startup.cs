using Application;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TanvirArjel.EFCore.GenericRepository;

namespace Api
{
    public class Startup
    {
        private static string Auth0Domain  = string.Empty;
        private static string Auth0Audience = string.Empty;
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
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

            //Auth0
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static void InitServices(IServiceCollection services)
        {
            // Register all mediator dependencies.
            services.AddApplication();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddSingleton<ISettings, UserSettings>();
            services.AddGenericRepository<PreferencesDbContext>();
            services.AddQueryRepository<PreferencesDbContext>();
        }

        public static void InitAuth0(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
                {
                    // TODO: Remove hardcoded auth0 settings
                    c.Authority = $"https://preferences.test.com";
                    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudience = "https://localhost:7122",
                        ValidIssuer = "https://preferences.test.com"
                    };
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("admin:read-write", p => p.
                    RequireAuthenticatedUser().
                    RequireClaim("permissions", "admin:read-write"));
                o.AddPolicy("manager:read-write", p => p.
                    RequireAuthenticatedUser().
                    RequireClaim("permissions", "manager:read-write"));
                o.AddPolicy("user:read-write", p => p.
                    RequireAuthenticatedUser().
                    RequireClaim("permissions", "user:read-write"));
            });
        }

        public static void InitContext(IServiceCollection services)
        {
            services.AddDbContext<PreferencesDbContext>();
        }
    }
}