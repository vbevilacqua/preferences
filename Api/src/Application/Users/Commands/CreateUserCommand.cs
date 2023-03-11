using Application.Solutions.Commands;
using Application.Users.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Users.Commands
{
    public class CreateUserCommand : IRequest<UserResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userDal = new User
            {
                Name = request.Name,
                Email = request.Email,
                CreatedDate = DateTime.Now.ToUniversalTime(),
                
            };

            this._repository.Add(userDal);
            await this._repository.SaveChangesAsync();
            return _mapper.Map<UserResponse>(userDal);
        }
    }
    
}