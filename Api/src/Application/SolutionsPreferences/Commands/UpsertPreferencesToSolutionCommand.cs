using System.Text.Json.Serialization;
using Application.Solutions.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.SolutionsPreferences.Commands
{
    public class UpsertPreferencesToSolutionCommand : IRequest<SolutionResponse>
    {
        [JsonIgnore]
        public Int32 SolutionId { get; set; }

        public IEnumerable<SolutionPreferenceCommand> SolutionPreferences { get; set; }
    }

    public class UpsertPreferencesToSolutionCommandHandler : IRequestHandler<UpsertPreferencesToSolutionCommand, SolutionResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpsertPreferencesToSolutionCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SolutionResponse> Handle(UpsertPreferencesToSolutionCommand request, CancellationToken cancellationToken)
        {
            var solution = await _repository.GetByIdAsync<Solution>(request.SolutionId);
            solution.SolutionPreferences = solution.SolutionPreferences ?? new List<SolutionPreference>();

            ValidateUpsertIds(request, solution);

            solution.SolutionPreferences.Clear();

            foreach (var command in request.SolutionPreferences)
            {
                var preference = new SolutionPreference
                {
                    Name = command.Name,
                    Value = command.Value,
                    IsActive = true
                };

                if (command.Id != null)
                {
                    preference.Id = command.Id.Value;
                }

                solution.SolutionPreferences.Add(preference);
            }

            return _mapper.Map<SolutionResponse>(solution);
        }

        private static void ValidateUpsertIds(UpsertPreferencesToSolutionCommand request, Solution solution)
        {
            var allIds = request.SolutionPreferences.Where(p => p.Id != null).Select(p => p.Id);
            var invalidIds = allIds.Where(id => !solution.SolutionPreferences.Any(dbPreference => dbPreference.Id == id.Value));

            if (invalidIds.Any())
            {
                string friendlyAllIds = string.Join(",", invalidIds.Select(id => id.Value));
                throw new InvalidIdException($"These preferences {friendlyAllIds} aren't present on the database.");
            }
        }
    }
}
