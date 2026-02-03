using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListProjectIdeasQuery : IRequest<IList<ListProjectIdeasViewModel>>
    {
        #region Query
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListProjectIdeasQuery, IList<ListProjectIdeasViewModel>>
        {
            private readonly IProjectIdeaRepository projectIdeaRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectIdeaRepository projectIdeaRepository, IMapper mapper)
            {
                this.projectIdeaRepository = projectIdeaRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListProjectIdeasViewModel>> Handle(ListProjectIdeasQuery request, CancellationToken cancellationToken)
            {
                var projectideas = await projectIdeaRepository.ListProjectIdeasAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListProjectIdeasViewModel>>(projectideas).ToList();
            }
        }
        #endregion
    }
}
