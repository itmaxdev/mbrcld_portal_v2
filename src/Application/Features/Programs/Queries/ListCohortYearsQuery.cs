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
    public sealed class ListCohortYearsQuery : IRequest<IList<int>>
    {
        #region Query
        public string programId { get; set; }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListCohortYearsQuery, IList<int>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.mapper = mapper;
            }

            public async Task<IList<int>> Handle(ListCohortYearsQuery request, CancellationToken cancellationToken)
            {
                var programId = new Guid(request.programId);
                var years = await programRepository.ListDistinctCohortYearsAsync(programId,cancellationToken);
                return years;
            }
        }
        #endregion
    }
}
