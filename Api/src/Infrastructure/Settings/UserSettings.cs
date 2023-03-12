using Domain.Common;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Settings
{
    public class UserSettings : ISettings
    {
        private IConfiguration configuration;

        public UserSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // public string DbConnection => configuration["DbConnection"];
        public string DbConnection => "User ID=root;Password=root;Host=localhost;Port=5432;Database=preferences;Pooling=true;Include Error Detail=True;";
    }
}