namespace Application.Solutions.Commands
{
    using System;
    using System.Threading.Tasks;
    using Application.Solutions.Queries;
    using AutoMapper;
    using MediatR;
    using TanvirArjel.EFCore.GenericRepository;
    using Domain.Entities;

    public class CreateSolutionCommand : IRequest<SolutionResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }

    public class CreateSolutionHandler : IRequestHandler<CreateSolutionCommand, SolutionResponse>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateSolutionHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SolutionResponse> Handle(CreateSolutionCommand request, CancellationToken cancellationToken)
        {
            var solutionDal = new Solution
            {
                CreatedDate = DateTime.Now.ToUniversalTime(),
                Name = request.Name,
                Type = request.Type,
            };

            this._repository.Add(solutionDal);
            await this._repository.SaveChangesAsync();
            return _mapper.Map<SolutionResponse>(solutionDal);
        }
    }
}
