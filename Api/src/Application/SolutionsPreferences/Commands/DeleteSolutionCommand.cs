using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.SolutionsPreferences.Commands
{

    public class DeleteSolutionCommand: IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteSolutionCommandHandler : IRequestHandler<DeleteSolutionCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteSolutionCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteSolutionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync<Solution>(request.Id);

            if (entity == null)
            {
                throw new NotFoundException($"User id {request.Id} not found.");
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}