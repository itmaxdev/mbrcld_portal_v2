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
    public sealed class ListTeamMembersByModuleIdQuery : IRequest<IList<ListTeamMembersByModuleIdViewModel>>
    {
        #region Query
        public Guid ModuleId { get; }

        public ListTeamMembersByModuleIdQuery(Guid moduleId)
        {
            ModuleId = moduleId;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListTeamMembersByModuleIdQuery, IList<ListTeamMembersByModuleIdViewModel>>
        {
            private readonly IUniversityTeamMemberRepository teamMemberRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUniversityTeamMemberRepository teamMemberRepository, IMapper mapper)
            {
                this.teamMemberRepository = teamMemberRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListTeamMembersByModuleIdViewModel>> Handle(ListTeamMembersByModuleIdQuery request, CancellationToken cancellationToken)
            {
                var teamMembers = await teamMemberRepository.ListTeamMembersByModuleIdAsync(request.ModuleId, cancellationToken);
                return mapper.Map<IEnumerable<ListTeamMembersByModuleIdViewModel>>(teamMembers).ToList();
            }
        }
        #endregion
    }
}
