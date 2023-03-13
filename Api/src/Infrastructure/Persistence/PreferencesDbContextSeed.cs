using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class PreferencesDbContextSeed
    {
        public static async Task SeedSampleDataAsync(PreferencesDbContext context)
        {
            // Seed, if necessary
            if (!context.UniversalPreferences.Any())
            {
                context.UniversalPreferences.Add(new GlobalPreference
                {
                    Name = "Dark Mode",
                    Value = "True",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                });
                context.UniversalPreferences.Add(new GlobalPreference
                {
                    Name = "Default Settings",
                    Value = "False",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                });
                context.UniversalPreferences.Add(new GlobalPreference
                {
                    Name = "Newsletter",
                    Value = "False",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
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

            if (!context.SolutionPreferences.Any())
            {
                context.SolutionPreferences.Add(new SolutionPreference
                {
                    Name = "Dark Mode",
                    Value = "True",
                    IsActive = true,
                    SolutionId = 2
                });
                context.SolutionPreferences.Add(new SolutionPreference
                {
                    Name = "Default Settings",
                    Value = "True",
                    IsActive = true,
                    SolutionId = 2
                });

                await context.SaveChangesAsync();
            }
            
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Name = "User A", Email = "test@test.com", CreatedDate = DateTime.UtcNow });
                context.Users.Add(new User { Name = "User B", Email = "test@test.com", CreatedDate = DateTime.UtcNow });

                await context.SaveChangesAsync();
            }
            
            if (!context.UserPreferences.Any())
            {
                context.UserPreferences.Add(new UserPreference { Name = "Dark Mode", UserId = 1, SolutionId = 1, Value = "False" });
                context.UserPreferences.Add(new UserPreference { Name = "Default Settings", UserId = 1, SolutionId = 1, Value = "True" });

                await context.SaveChangesAsync();
            }
        }
    }
}
