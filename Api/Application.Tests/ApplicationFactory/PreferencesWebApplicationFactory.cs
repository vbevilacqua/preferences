namespace Api.Tests.ApplicationFactory
{
    using Infrastructure.Persistence;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using TanvirArjel.EFCore.GenericRepository;

    public class PreferencesWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<PreferencesIntegrationContext>(options =>
                {
                    options.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });

                services.AddGenericRepository<PreferencesIntegrationContext>();
                services.AddQueryRepository<PreferencesIntegrationContext>();


                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = FakeJwtManager.SecurityKey,
                        ValidIssuer = FakeJwtManager.Issuer,
                        ValidAudience = FakeJwtManager.Audience
                    };
                });
            });

            builder.UseEnvironment("Test");
        }

        public async Task InitSeed()
        {
            var context = this.Services.GetRequiredService<PreferencesIntegrationContext>();

            context.Database.EnsureCreated();

            await PreferencesDbContextSeed.SeedSampleDataAsync(context);
        }

        public async Task DeleteDatabaseAsync()
        {
        
        }
    }
}