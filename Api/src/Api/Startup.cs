using Application;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Settings;
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
        }

        public static void InitServices(IServiceCollection services)
        {
            // Register all mediator dependnecies.
            services.AddApplication();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddSingleton<ISettings, UserSettings>();
            services.AddGenericRepository<UserSettingsDbContext>();
            services.AddQueryRepository<UserSettingsDbContext>();
        }

        public static void InitContext(IServiceCollection services)
        {
            services.AddDbContext<UserSettingsDbContext>();
        }
    }
}