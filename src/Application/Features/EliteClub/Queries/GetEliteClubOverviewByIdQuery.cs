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
    public sealed class GetEliteClubOverviewByIdQuery : IRequest<GetEliteClubOverviewByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetEliteClubOverviewByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetEliteClubOverviewByIdQuery, GetEliteClubOverviewByIdViewModel>
        {
            private readonly IEliteClubRepository eliteClubRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEliteClubRepository eliteClubRepository, IMapper mapper)
            {
                this.eliteClubRepository = eliteClubRepository;
                this.mapper = mapper;
            }

            public async Task<GetEliteClubOverviewByIdViewModel> Handle(GetEliteClubOverviewByIdQuery request, CancellationToken cancellationToken)
            {
                var eliteClub = await eliteClubRepository.GetEliteClubByIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<GetEliteClubOverviewByIdViewModel>(eliteClub.ValueOrDefault);
            }
        }
        #endregion
    }
}
