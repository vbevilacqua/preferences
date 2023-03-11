using AutoMapper;
using Domain.Entities;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserResponse>>
    {
        public Int32? UserId { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == null)
            {
                var result = await _repository.GetListAsync<User>();
                return _mapper.Map<IEnumerable<UserResponse>>(result);              
            }
            else
            {
                var result = new List<User>();
                result.Add(await _repository.GetByIdAsync<User>(request.UserId));
                return _mapper.Map<IEnumerable<UserResponse>>(result);
            }
        }
    }
}