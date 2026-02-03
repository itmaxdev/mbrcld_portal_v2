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
    public sealed class SearchProjectIdeasQuery : IRequest<IList<SearchProjectIdeasViewModel>>
    {
        #region Query
        public string Search { get; }

        public SearchProjectIdeasQuery(string search)
        {
            Search = search;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<SearchProjectIdeasQuery, IList<SearchProjectIdeasViewModel>>
        {
            private readonly IProjectIdeaRepository projectIdeaRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectIdeaRepository projectIdeaRepository, IMapper mapper)
            {
                this.projectIdeaRepository = projectIdeaRepository;
                this.mapper = mapper;
            }

            public async Task<IList<SearchProjectIdeasViewModel>> Handle(SearchProjectIdeasQuery request, CancellationToken cancellationToken)
            {
                var projectideas = await projectIdeaRepository.SearchProjectIdeasAsync(request.Search, cancellationToken);
                return mapper.Map<IEnumerable<SearchProjectIdeasViewModel>>(projectideas).ToList();
            }
        }
        #endregion
    }
}
