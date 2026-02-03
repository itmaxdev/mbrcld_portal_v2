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
    public sealed class GetAlumniEliteClubQuery : IRequest<GetAlumniEliteClubViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetAlumniEliteClubQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetAlumniEliteClubQuery, GetAlumniEliteClubViewModel>
        {
            private readonly IEliteClubRepository eliteClubRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEliteClubRepository eliteClubRepository, IMapper mapper)
            {
                this.eliteClubRepository = eliteClubRepository;
                this.mapper = mapper;
            }

            public async Task<GetAlumniEliteClubViewModel> Handle(GetAlumniEliteClubQuery request, CancellationToken cancellationToken)
            {
                GetAlumniEliteClubViewModel eliteClub = null;
                var eliteClubs = await eliteClubRepository.GetAlumniEliteClubAsync(request.Id).ConfigureAwait(false);
                if (eliteClubs.HasValue)
                {
                    eliteClub = mapper.Map<GetAlumniEliteClubViewModel>(eliteClubs.Value);
                    eliteClub.IsActiveMember = true;
                }
                return eliteClub;
            }
        }
        #endregion
    }
}
