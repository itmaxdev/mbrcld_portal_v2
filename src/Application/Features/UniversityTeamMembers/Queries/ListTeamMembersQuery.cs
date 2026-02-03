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
    public sealed class ListTeamMembersQuery : IRequest<IList<ListTeamMembersViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListTeamMembersQuery(Guid userId)
        {
            UserId = userId;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListTeamMembersQuery, IList<ListTeamMembersViewModel>>
        {
            private readonly IUniversityTeamMemberRepository teamMemberRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUniversityTeamMemberRepository teamMemberRepository, IMapper mapper)
            {
                this.teamMemberRepository = teamMemberRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListTeamMembersViewModel>> Handle(ListTeamMembersQuery request, CancellationToken cancellationToken)
            {
                var teamMemberss = await teamMemberRepository.ListTeamMembersAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListTeamMembersViewModel>>(teamMemberss).ToList();
            }
        }
        #endregion
    }
}
