using System.Text.Json.Serialization;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.SolutionsPreferences.Commands
{

    public class UpdateSolutionCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Type { get; set; }
    }
    
    public class UpdateSolutionCommandHandler : IRequestHandler<UpdateSolutionCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateSolutionCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateSolutionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync<Solution>(request.Id);

            if (entity == null)
            {
                throw new NotFoundException($"Solution id {request.Id} not found.");
            }

            entity.Name = request.Name;

            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}