using System.Text.Json.Serialization;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.GlobalPreferences.Commands{

    public class UpdateGlobalPreferenceCommand: IRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
    
    public class UpdateGlobalPreferenceCommandHandler : IRequestHandler<UpdateGlobalPreferenceCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateGlobalPreferenceCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateGlobalPreferenceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync<GlobalPreference>(request.Name);
            if (entity == null)
            {
                throw new NotFoundException($"Global Preference name {request.Name} not found.");
            }

            entity.Value = request.Value;
            entity.IsActive = request.IsActive;
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}