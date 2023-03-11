using System.Text.Json.Serialization;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Users.Commands{

    public class UpdateUserCommand: IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
    }
    
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync<User>(request.Id);

            if (entity == null)
            {
                throw new NotFoundException($"User id {request.Id} not found.");
            }

            entity.Name = request.Name;

            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}