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
    public sealed class GetTeamMemberByIdQuery : IRequest<GetTeamMemberByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetTeamMemberByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetTeamMemberByIdQuery, GetTeamMemberByIdViewModel>
        {
            private readonly IUniversityTeamMemberRepository teamMemberRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUniversityTeamMemberRepository teamMemberRepository, IMapper mapper)
            {
                this.teamMemberRepository = teamMemberRepository;
                this.mapper = mapper;
            }

            public async Task<GetTeamMemberByIdViewModel> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
            {
                var teamMember = await teamMemberRepository.GetTeamMemberByIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<GetTeamMemberByIdViewModel>(teamMember.ValueOrDefault);
            }
        }
        #endregion
    }
}
