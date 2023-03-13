namespace Application.GlobalPreferences.Services
{
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IGlobalPreferencesService
    {
        Task<GlobalPreference?> GetByNameAsync(string name);
    }
}