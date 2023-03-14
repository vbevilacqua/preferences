using Domain.Entities;

namespace Application.GlobalPreferences.Services
{
    public interface IGlobalPreferencesService
    {
        Task<GlobalPreference?> GetByNameAsync(string name);
    }
}