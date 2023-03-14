namespace Api.Tests.ApplicationFactory
{
    using Domain.Common;
    using Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;

    public class PreferencesIntegrationContext : PreferencesDbContext
    {
        public PreferencesIntegrationContext(ISettings settings) : base(settings)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "preferences1");

        }
    }
}