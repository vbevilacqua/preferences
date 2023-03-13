﻿namespace Application.GlobalPreferences.Services
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using TanvirArjel.EFCore.GenericRepository;

    public class GlobalPreferencesService : IGlobalPreferencesService
    {
        private readonly IRepository _repository;

        public GlobalPreferencesService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<GlobalPreference?> GetByNameAsync(string name)
        {
            var specification = new Specification<GlobalPreference>();
            specification.Conditions.Add(e => e.Name == name);
            var result = await _repository.GetListAsync(specification);
            return result.FirstOrDefault();
        }
    }
}
