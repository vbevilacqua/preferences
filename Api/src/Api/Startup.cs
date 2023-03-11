using Application;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;
using Infrastructure.Settings;
using Infrastructure.Persistence;

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
            // Register all mediator dependnecies.
            services.AddApplication();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddDbContext<UserSettingsDbContext>();
            services.AddSingleton<ISettings, UserSettings>();
            services.AddGenericRepository<UserSettingsDbContext>();
            services.AddQueryRepository<UserSettingsDbContext>();
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
    }
}