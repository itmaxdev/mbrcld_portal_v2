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
    public sealed class ListUserSpecialProjectsQuery : IRequest<IList<ListUserSpecialProjectsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListUserSpecialProjectsQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListUserSpecialProjectsQuery, IList<ListUserSpecialProjectsViewModel>>
        {
            private readonly ISpecialProjectRepository specialProjectRepository;
            private readonly IMapper mapper;

            public QueryHandler(ISpecialProjectRepository specialProjectRepository, IMapper mapper)
            {
                this.specialProjectRepository = specialProjectRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserSpecialProjectsViewModel>> Handle(ListUserSpecialProjectsQuery request, CancellationToken cancellationToken)
            {
                var specialProjects = await specialProjectRepository.ListUserSpecialProjectsAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListUserSpecialProjectsViewModel>>(specialProjects).ToList();
            }
        }
        #endregion
    }
}
