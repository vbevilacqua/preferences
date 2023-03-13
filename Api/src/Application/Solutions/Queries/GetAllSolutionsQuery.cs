using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Solutions.Queries
{
    public class GetAllSolutionsQuery : IRequest<IEnumerable<SolutionResponse>>
    {
    }

    public class GetAllSolutionsQueryHandler : IRequestHandler<GetAllSolutionsQuery, IEnumerable<SolutionResponse>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSolutionsQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SolutionResponse>> Handle(GetAllSolutionsQuery request, CancellationToken cancellationToken)
        {
            var specification = new Specification<Solution>();
            specification.Includes = ep => ep.Include(e => e.SolutionPreferences);
            var result = await _repository.GetListAsync(specification);
            return _mapper.Map<IEnumerable<SolutionResponse>>(result);
        }
    }
}