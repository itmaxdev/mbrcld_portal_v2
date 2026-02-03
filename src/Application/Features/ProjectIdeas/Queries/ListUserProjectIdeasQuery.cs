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
    public sealed class ListUserProjectIdeasQuery : IRequest<IList<ListUserProjectIdeasViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListUserProjectIdeasQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListUserProjectIdeasQuery, IList<ListUserProjectIdeasViewModel>>
        {
            private readonly IProjectIdeaRepository projectIdeaRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProjectIdeaRepository projectIdeaRepository, IMapper mapper)
            {
                this.projectIdeaRepository = projectIdeaRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserProjectIdeasViewModel>> Handle(ListUserProjectIdeasQuery request, CancellationToken cancellationToken)
            {
                var projectideas = await projectIdeaRepository.ListUserProjectIdeasAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListUserProjectIdeasViewModel>>(projectideas).ToList();
            }
        }
        #endregion
    }
}
