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
    public sealed class ListInstructorProjectsQuery : IRequest<IList<ListInstructorProjectsViewModel>>
    {
        #region Query
        public Guid UserId { get; }
        public Guid Id { get; }

        public ListInstructorProjectsQuery(Guid userid , Guid id)
        {
            UserId = userid;
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListInstructorProjectsQuery, IList<ListInstructorProjectsViewModel>>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListInstructorProjectsViewModel>> Handle(ListInstructorProjectsQuery request, CancellationToken cancellationToken)
            {
                var projects = await projectRepository.ListInstructorProjectsAsync(request.UserId, request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListInstructorProjectsViewModel>>(projects).ToList();
            }
        }
        #endregion
    }
}
