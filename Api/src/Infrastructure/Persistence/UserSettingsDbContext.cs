using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class UserSettingsDbContext : DbContext
    {
        private readonly ISettings settings;

        public UserSettingsDbContext(ISettings settings)
        {
            this.settings = settings;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(settings.DbConnection);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Solution> Solutions { get; set; } = null!;
        public DbSet<SolutionPreference> SolutionPreferences { get; set; } = null!;
        public DbSet<GlobalPreference> UniversalPreferences { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPreference> UserPreferences { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolutionPreference>().HasKey(m => new { m.Name, m.SolutionId });
            modelBuilder.Entity<UserPreference>().HasKey(m => new { m.Name, m.UserId, m.SolutionId });
        }
    }
}