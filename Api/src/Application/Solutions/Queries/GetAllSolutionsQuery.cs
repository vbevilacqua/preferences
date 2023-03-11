using AutoMapper;
using Domain.Entities;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.Solutions.Queries
{
    public class GetAllSolutionsQuery : IRequest<IEnumerable<SolutionResponse>>
    {
        public Int32? SolutionId { get; set; }
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
            if (request.SolutionId == null)
            {
                var result = await _repository.GetListAsync<Solution>();
                return _mapper.Map<IEnumerable<SolutionResponse>>(result);                
            }
            else
            {
                var result = new List<Solution>();
                result.Add(await _repository.GetByIdAsync<Solution>(request.SolutionId));
                return _mapper.Map<IEnumerable<SolutionResponse>>(result);
            }
        }
    }
}