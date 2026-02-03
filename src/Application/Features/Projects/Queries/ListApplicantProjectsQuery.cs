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
    public sealed class ListApplicantProjectsQuery : IRequest<IList<ListApplicantProjectsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListApplicantProjectsQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListApplicantProjectsQuery, IList<ListApplicantProjectsViewModel>>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListApplicantProjectsViewModel>> Handle(ListApplicantProjectsQuery request, CancellationToken cancellationToken)
            {
                var projects = await projectRepository.ListProjectsAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListApplicantProjectsViewModel>>(projects).ToList();
            }
        }
        #endregion
    }
}
