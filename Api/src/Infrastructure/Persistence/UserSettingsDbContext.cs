using Domain.Entities;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class UserSettingsDbContext : DbContext
    {
        private readonly ISettings settings;

        public UserSettingsDbContext(ISettings settings)
        {
            this.settings = settings;
            Database.SetConnectionString(settings.DbConnection);
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(settings.DbConnection);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Solution> Solutions { get; set; } = null!;
        public DbSet<SolutionPreference> SolutionPreferences { get; set; } = null!;
        public DbSet<UniversalPreference> UniversalPreferences { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPreference> UserPreferences { get; set; } = null!;
    }
}