using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetEliteMembershipAttendanceQuery : IRequest<IList<GetEliteMembershipAttendanceViewModel>>
    {
        #region Query
        public Guid UserId { get; }
        public Guid EliteClubId { get; }
        public int MembershipType { get; }

        public GetEliteMembershipAttendanceQuery(Guid userId , Guid eliteClubId, int membershipType)
        {
            UserId = userId;
            EliteClubId = eliteClubId;
            MembershipType = membershipType;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetEliteMembershipAttendanceQuery, IList<GetEliteMembershipAttendanceViewModel>>
        {
            private readonly IEliteMembershipAttendanceRepository eliteMembershipAttendanceRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEliteMembershipAttendanceRepository eliteMembershipAttendanceRepository, IMapper mapper)
            {
                this.eliteMembershipAttendanceRepository = eliteMembershipAttendanceRepository;
                this.mapper = mapper;
            }

            public async Task<IList<GetEliteMembershipAttendanceViewModel>> Handle(GetEliteMembershipAttendanceQuery request, CancellationToken cancellationToken)
            {
                var eliteMembershipAttendance = await eliteMembershipAttendanceRepository.GetEliteMembershipAttendanceAsync(request.UserId, request.EliteClubId, request.MembershipType).ConfigureAwait(false);
                return mapper.Map<IList<GetEliteMembershipAttendanceViewModel>>(eliteMembershipAttendance.ToList());
            }
        }
        #endregion
    }
}
