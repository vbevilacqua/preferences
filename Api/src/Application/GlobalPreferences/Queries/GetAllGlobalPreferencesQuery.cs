using AutoMapper;
using Domain.Entities;
using MediatR;
using TanvirArjel.EFCore.GenericRepository;

namespace Application.GlobalPreferences.Queries
{
    public class GetAllGlobalPreferencesQuery : IRequest<IEnumerable<GlobalPreferenceResponse>>
    {
    }

    public class GetAllGlobalPreferencesQueryHandler : IRequestHandler<GetAllGlobalPreferencesQuery, IEnumerable<GlobalPreferenceResponse>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllGlobalPreferencesQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GlobalPreferenceResponse>> Handle(GetAllGlobalPreferencesQuery request, CancellationToken cancellationToken)
        {
            var specification = new Specification<GlobalPreference>();
            var result = await _repository.GetListAsync<GlobalPreference>(specification, cancellationToken);
            return _mapper.Map<IEnumerable<GlobalPreferenceResponse>>(result);
        }
    }
}