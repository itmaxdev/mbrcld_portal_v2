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
    public sealed class SearchSpecialProjectsQuery : IRequest<IList<SearchSpecialProjectsViewModel>>
    {
        #region Query
        public string Search { get; }

        public SearchSpecialProjectsQuery(string search)
        {
            Search = search;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<SearchSpecialProjectsQuery, IList<SearchSpecialProjectsViewModel>>
        {
            private readonly ISpecialProjectRepository specialProjectRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISpecialProjectRepository specialProjectRepository, IMapper mapper)
            {
                this.specialProjectRepository = specialProjectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<SearchSpecialProjectsViewModel>> Handle(SearchSpecialProjectsQuery request, CancellationToken cancellationToken)
            {
                var spcialProjects = await specialProjectRepository.SearchSpecialProjectsAsync(request.Search, cancellationToken);
                return mapper.Map<IEnumerable<SearchSpecialProjectsViewModel>>(spcialProjects).ToList();
            }
        }
        #endregion
    }
}
