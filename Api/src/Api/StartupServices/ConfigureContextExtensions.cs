namespace Api.StartupServices
{
    using Infrastructure.Persistence;
    using TanvirArjel.EFCore.GenericRepository;

    public static class ConfigureContextExtensions
    {
        public static IServiceCollection ConfigureContext(this IServiceCollection services)
        {
            services.AddDbContext<PreferencesDbContext>();
            services.AddGenericRepository<PreferencesDbContext>();
            services.AddQueryRepository<PreferencesDbContext>();
            return services;
        }
    }
}

