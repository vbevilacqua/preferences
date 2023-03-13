using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserResponse>
    {
        public Int32? UserId { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new Specification<User>();
            specification.Conditions.Add(e => e.Id.Equals(request.UserId));
            specification.Includes = ep => ep.Include(e => e.UserPreferences);
            var result = await _repository.GetAsync<User>(specification, cancellationToken);
            if (result == null)
            {
                throw new NotFoundException($"User id {request.UserId} not found.");
            }
            return _mapper.Map<UserResponse>(result);
        }
    }
}