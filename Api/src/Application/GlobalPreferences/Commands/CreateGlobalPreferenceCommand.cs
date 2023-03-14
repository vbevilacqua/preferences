using Application.GlobalPreferences.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.GlobalPreferences.Commands
{
    public class CreateGlobalPreferenceCommand : IRequest<GlobalPreferenceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; } 
    }

    public class CreateGlobalPreferenceCommandHandler : IRequestHandler<CreateGlobalPreferenceCommand, GlobalPreferenceResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateGlobalPreferenceCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GlobalPreferenceResponse> Handle(CreateGlobalPreferenceCommand request, CancellationToken cancellationToken)
        {
            var globalPreferenceDal = new GlobalPreference
            {
                Name = request.Name,
                Value = request.Value,
                IsActive = request.IsActive,
                CreatedDate = DateTime.Now.ToUniversalTime(),
                
            };

            this._repository.Add(globalPreferenceDal);
            await this._repository.SaveChangesAsync();
            return _mapper.Map<GlobalPreferenceResponse>(globalPreferenceDal);
        }
    }
    
}