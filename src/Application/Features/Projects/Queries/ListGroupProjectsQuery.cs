using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListGroupProjectsQuery : IRequest<IList<ListGroupProjectsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListGroupProjectsQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListGroupProjectsQuery, IList<ListGroupProjectsViewModel>>
        {
            private readonly IProjectRepository projectRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectRepository projectRepository, IMapper mapper)
            {
                this.projectRepository = projectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListGroupProjectsViewModel>> Handle(ListGroupProjectsQuery request, CancellationToken cancellationToken)
            {
                List<Project> GroupProject = new List<Project> { };

                var leadprojects = await projectRepository.ListLeadProjectsAsync(request.UserId, cancellationToken);
                foreach(var leadproject in leadprojects)
                {
                    leadproject.IsParent = false;
                    GroupProject.Add(leadproject);
                }

                var parentprojects = await projectRepository.ListParentProjectsAsync(request.UserId, cancellationToken);
                foreach(var parentproject in parentprojects)
                {
                    parentproject.IsParent = true;
                    GroupProject.Add(parentproject);
                }

                return mapper.Map<IEnumerable<ListGroupProjectsViewModel>>(GroupProject).ToList();
            }
        }
        #endregion
    }
}
