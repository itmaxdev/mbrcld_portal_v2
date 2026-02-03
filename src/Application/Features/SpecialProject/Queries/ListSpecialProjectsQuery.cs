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
    public sealed class ListSpecialProjectsQuery : IRequest<IList<ListSpecialProjectsViewModel>>
    {
        #region Query
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListSpecialProjectsQuery, IList<ListSpecialProjectsViewModel>>
        {
            private readonly ISpecialProjectRepository specialProjectRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISpecialProjectRepository specialProjectRepository, IMapper mapper)
            {
                this.specialProjectRepository = specialProjectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListSpecialProjectsViewModel>> Handle(ListSpecialProjectsQuery request, CancellationToken cancellationToken)
            {
                var specialProjects = await specialProjectRepository.ListSpecialProjectsAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListSpecialProjectsViewModel>>(specialProjects).ToList();
            }
        }
        #endregion
    }
}
