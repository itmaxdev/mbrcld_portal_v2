using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Memberships.Queries
{
    public sealed class ListUserMembershipsQuery : IRequest<IList<ListUserMembershipsViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public bool Completed { get; set; }

        public ListUserMembershipsQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserMembershipsQuery, IList<ListUserMembershipsViewModel>>
        {
            private readonly IMembershipRepository membershipRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMembershipRepository membershipRepository, IMapper mapper)
            {
                this.membershipRepository = membershipRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserMembershipsViewModel>> Handle(ListUserMembershipsQuery request, CancellationToken cancellationToken)
            {
                var Memberships = await membershipRepository.ListByUserIdAsync(request.Id, cancellationToken);
                return mapper.Map<IList<ListUserMembershipsViewModel>>(Memberships);
            }
        }
        #endregion
    }
}
