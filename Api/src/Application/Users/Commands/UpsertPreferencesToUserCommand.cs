using System.Text.Json.Serialization;
using Application.GlobalPreferences.Services;
using Application.Users.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Users.Commands
{

    public class UpsertPreferencesToUserCommand : IRequest<UserResponse>
    {
        [JsonIgnore]
        public Int32 UserId { get; set; }
        [JsonIgnore]
        public Int32 SolutionId { get; set; }

        public IEnumerable<UserPreferenceDTO> UserPreferences { get; set; }
    }

    public class UpsertPreferencesToUserCommandHandler : IRequestHandler<UpsertPreferencesToUserCommand, UserResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IGlobalPreferencesService _globalPreferencesService;

        public UpsertPreferencesToUserCommandHandler(IRepository repository, IMapper mapper, IGlobalPreferencesService globalPreferencesService)
        {
            _repository = repository;
            _mapper = mapper;
            _globalPreferencesService = globalPreferencesService;
        }

        public async Task<UserResponse> Handle(UpsertPreferencesToUserCommand request, CancellationToken cancellationToken)
        {
            var solution = await FindSolution(request);
            var user = await FindUser(request);


            user.UserPreferences = user.UserPreferences ?? new List<UserPreference>();
            // ValidateUpsertIds(request, user);
            user.UserPreferences.Clear();
            UserResponse result = _mapper.Map<UserResponse>(user);

            foreach (var command in request.UserPreferences)
            {
                var globalPreference = await _globalPreferencesService.GetByNameAsync(command.Name);
                if (globalPreference == null)
                {
                    throw new GlobalPreferenceDoesNotExist($"The solution preference {command.Name} must to be added for global first.");
                }
                
                var preference = new UserPreference
                {
                    Name = command.Name,
                    Value = command.Value,
                    IsActive = true,
                    SolutionId = solution.Id,
                    UserId = user.Id
                };

                result.UserPreferences.Add(preference);
            }

            await _repository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserResponse>(result);
        }

        private async Task<Solution> FindSolution(UpsertPreferencesToUserCommand request)
        {
            var solution = await _repository.GetByIdAsync<Solution>(request.SolutionId);
            if (solution == null)
            {
                throw new InvalidIdException($"Solution {request.SolutionId} isn't present on the database.");
            }

            return solution;
        }

        private async Task<User> FindUser(UpsertPreferencesToUserCommand request)
        {
            var user = await _repository.GetByIdAsync<User>(request.UserId);
            if (user == null)
            {
                throw new InvalidIdException($"User {request.SolutionId} isn't present on the database.");
            }

            return user;
        }
    }
}