using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class UserSettingsDbContextSeed
    {
        public static async Task SeedSampleDataAsync(UserSettingsDbContext context)
        {
            // Seed, if necessary
            if (!context.UniversalPreferences.Any())
            {
                context.UniversalPreferences.Add(new UniversalPreference
                {
                    Name = "User Consent",
                    Value = "True",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                });
                context.UniversalPreferences.Add(new UniversalPreference
                {
                    Name = "Newsletter",
                    Value = "False",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }

            if (!context.SolutionPreferences.Any())
            {
                context.SolutionPreferences.Add(new SolutionPreference
                {
                    Name = "Dark Mode",
                    Value = "True",
                    IsActive = true
                });
                context.SolutionPreferences.Add(new SolutionPreference
                {
                    Name = "Default Settings",
                    Value = "True",
                    IsActive = true
                });

                await context.SaveChangesAsync();
            }

            if (!context.Solutions.Any())
            {
                context.Solutions.Add(new Solution { Name = "Game A", Type = "Game", CreatedDate = DateTime.UtcNow });
                context.Solutions.Add(new Solution { Name = "App A", Type = "App", CreatedDate = DateTime.UtcNow });
                context.Solutions.Add(new Solution { Name = "Site A", Type = "Site", CreatedDate = DateTime.UtcNow });

                await context.SaveChangesAsync();
            }

        }
    }
}
