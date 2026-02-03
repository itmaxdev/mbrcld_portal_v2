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
    public sealed class SearchAlumniQuery : IRequest<IList<SearchAlumniViewModel>>
    {
        #region Query
        public string Search { get; }

        public SearchAlumniQuery(string search)
        {
            Search = search;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<SearchAlumniQuery, IList<SearchAlumniViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<IList<SearchAlumniViewModel>> Handle(SearchAlumniQuery request, CancellationToken cancellationToken)
            {
                var alumnies = await userRepository.SearchAlumniAsync(request.Search, cancellationToken);
                return mapper.Map<IEnumerable<SearchAlumniViewModel>>(alumnies).ToList();
            }
        }
        #endregion
    }
}
