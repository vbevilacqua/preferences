using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.GlobalPreferences.Commands
{
    public class DeleteGlobalPreferenceCommand : IRequest
    {
        public string Name { get; set; }
    }

    public class DeleteGlobalPreferenceCommandHandler : IRequestHandler<DeleteGlobalPreferenceCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteGlobalPreferenceCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteGlobalPreferenceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync<GlobalPreference>(request.Name);

            if (entity == null)
            {
                throw new NotFoundException($"Global Preference name {request.Name} not found.");
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}