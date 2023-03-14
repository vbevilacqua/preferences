using Api.StartupServices;
using Api.Swagger;

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
            services
                .ConfigureContext()
                .ConfigureLogic()
                .ConfigureAuth(Configuration);
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
    }
}