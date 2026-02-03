using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetProjectIdeaByIdQuery : IRequest<GetProjectIdeaByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetProjectIdeaByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetProjectIdeaByIdQuery, GetProjectIdeaByIdViewModel>
        {
            private readonly IProjectIdeaRepository projectIdeaRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectIdeaRepository projectIdeaRepository, IMapper mapper)
            {
                this.projectIdeaRepository = projectIdeaRepository;
                this.mapper = mapper;
            }

            public async Task<GetProjectIdeaByIdViewModel> Handle(GetProjectIdeaByIdQuery request, CancellationToken cancellationToken)
            {
                var projectIdea = await projectIdeaRepository.GetProjectIdeaByIdAsync(request.Id, cancellationToken);
                return mapper.Map<GetProjectIdeaByIdViewModel>(projectIdea.ValueOrDefault);
            }
        }
        #endregion
    }
}
