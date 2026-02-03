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
    public sealed class ListProjectsByCohortIdQuery : IRequest<IList<ListProjectsByCohortIdViewModel>>
    {
        #region Query
        public Guid CohortId { get; }
        public Guid UserId { get; }

        public ListProjectsByCohortIdQuery(Guid cohortid, Guid userid)
        {
            CohortId = cohortid;
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListProjectsByCohortIdQuery, IList<ListProjectsByCohortIdViewModel>>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListProjectsByCohortIdViewModel>> Handle(ListProjectsByCohortIdQuery request, CancellationToken cancellationToken)
            {
                var projects = await projectRepository.ListProjectsByCohortIdAsync(request.CohortId, request.UserId ,cancellationToken);
                return mapper.Map<IEnumerable<ListProjectsByCohortIdViewModel>>(projects).ToList();
            }
        }
        #endregion
    }
}
