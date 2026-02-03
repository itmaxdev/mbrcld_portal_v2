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
    public sealed class ListEliteClubMembersByIdQuery : IRequest<IList<ListEliteClubMembersByIdViewModel>>
    {
        #region Query
        public Guid EliteClubId { get; }

        public ListEliteClubMembersByIdQuery(Guid eliteClubId)
        {
            EliteClubId = eliteClubId;
        }
        #endregion
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListEliteClubMembersByIdQuery, IList<ListEliteClubMembersByIdViewModel>>
        {
            private readonly IEliteClubMemberRepository eliteClubMemberRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEliteClubMemberRepository eliteClubMemberRepository, IMapper mapper)
            {
                this.eliteClubMemberRepository = eliteClubMemberRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListEliteClubMembersByIdViewModel>> Handle(ListEliteClubMembersByIdQuery request, CancellationToken cancellationToken)
            {
                var eliteClubMembers = await eliteClubMemberRepository.ListEliteClubMembersByIdAsync(request.EliteClubId, cancellationToken);
                return mapper.Map<IEnumerable<ListEliteClubMembersByIdViewModel>>(eliteClubMembers).ToList();
            }
        }
        #endregion
    }
}
