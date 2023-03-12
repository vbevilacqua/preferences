using System.Text.Json.Serialization;
using Application.GlobalPreferences.Services;
using Application.Solutions.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.SolutionsPreferences.Commands
{
    public class UpsertPreferencesToSolutionCommand : IRequest<SolutionResponse>
    {
        [JsonIgnore]
        public Int32 SolutionId { get; set; }

        public ICollection<SolutionPreferenceDTO> SolutionPreferences { get; set; }
    }

    public class UpsertPreferencesToSolutionCommandHandler : IRequestHandler<UpsertPreferencesToSolutionCommand, SolutionResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IGlobalPreferencesService _globalPreferencesService;

        public UpsertPreferencesToSolutionCommandHandler(IRepository repository, IMapper mapper, IGlobalPreferencesService globalPreferencesService)
        {
            _repository = repository;
            _mapper = mapper;
            _globalPreferencesService = globalPreferencesService;
        }

        public async Task<SolutionResponse> Handle(UpsertPreferencesToSolutionCommand request, CancellationToken cancellationToken)
        {
            var solution = await FindSolution(request);

            foreach (var newSolutionPreference in request.SolutionPreferences)
            {
                var globalPreference = await _globalPreferencesService.GetByNameAsync(newSolutionPreference.Name);
                if (globalPreference == null)
                {
                    throw new GlobalPreferenceDoesNotExist($"The solution preference {newSolutionPreference.Name} must to be added for global first.");
                }

                var preference = solution.SolutionPreferences.FirstOrDefault(p => p.Name == newSolutionPreference.Name);
                if (preference == null)
                {
                    preference = new SolutionPreference();
                    preference.Name = newSolutionPreference.Name;
                    preference.Value = newSolutionPreference.Value;
                    preference.IsActive = true;
                    solution.SolutionPreferences.Add(preference);
                }
                else
                {
                    preference.Name = newSolutionPreference.Name;
                    preference.Value = newSolutionPreference.Value;
                    preference.IsActive = true;
                }
            }

            await _repository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<SolutionResponse>(solution);
        }

        private async Task<Solution> FindSolution(UpsertPreferencesToSolutionCommand request)
        {
            var specification = new Specification<Solution>();
            specification.Conditions.Add(e => e.Id == request.SolutionId);
            specification.Includes = ep => ep.Include(e => e.SolutionPreferences);

            var solution = await _repository.GetListAsync(specification);
            if (!solution.Any())
            {
                throw new InvalidIdException($"Solution {request.SolutionId} isn't present on the database.");
            }

            return solution.First();
        }

    }
}
