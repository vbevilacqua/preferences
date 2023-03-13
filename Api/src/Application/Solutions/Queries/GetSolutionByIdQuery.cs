using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Solutions.Queries
{
    public class GetSolutionByIdQuery : IRequest<SolutionResponse>
    {
        public Int32 SolutionId { get; set; }
    }

    public class GetSolutionByIdQueryHandler : IRequestHandler<GetSolutionByIdQuery, SolutionResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetSolutionByIdQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SolutionResponse> Handle(GetSolutionByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new Specification<Solution>();
            specification.Conditions.Add(e => e.Id.Equals(request.SolutionId));
            specification.Includes = ep => ep.Include(e => e.SolutionPreferences);
            var result = await _repository.GetAsync<Solution>(specification, cancellationToken);
            if (result == null)
            {
                throw new NotFoundException($"Solution id {request.SolutionId} not found.");
            }
            return _mapper.Map<SolutionResponse>(result);
        }
    }
}