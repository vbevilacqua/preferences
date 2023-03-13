using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var specification = new Specification<User>();
            specification.Includes = ep => ep.Include(e => e.UserPreferences);
            var result = await _repository.GetListAsync<User>(specification, cancellationToken);
            return _mapper.Map<IEnumerable<UserResponse>>(result);
        }
    }
}