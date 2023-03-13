namespace Api.Tests.ApplicationFactory
{
    using Infrastructure.Persistence;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    public class PreferencesWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                Startup.InitServices(services);
                services.AddDbContext<PreferencesIntegrationContext>(options =>
                {
                    options.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
            });

            builder.UseEnvironment("Test");
        }

        public async Task InitSeed()
        {
            var context = this.Services.GetRequiredService<PreferencesIntegrationContext>();
            await PreferencesDbContextSeed.SeedSampleDataAsync(context);
        }
    }
}