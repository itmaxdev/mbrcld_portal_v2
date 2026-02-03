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
    public sealed class ListApplicantCompletedProjectsQuery : IRequest<IList<ListApplicantCompletedProjectsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListApplicantCompletedProjectsQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListApplicantCompletedProjectsQuery, IList<ListApplicantCompletedProjectsViewModel>>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListApplicantCompletedProjectsViewModel>> Handle(ListApplicantCompletedProjectsQuery request, CancellationToken cancellationToken)
            {
                var projects = await projectRepository.ListCompletedProjectsAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListApplicantCompletedProjectsViewModel>>(projects).ToList();
            }
        }
        #endregion
    }
}
