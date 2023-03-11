using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync<User>(request.Id);

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