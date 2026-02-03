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
    public sealed class ListProgramByCohortContactQuery : IRequest<IList<ListProgramByCohortContactViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListProgramByCohortContactQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListProgramByCohortContactQuery, IList<ListProgramByCohortContactViewModel>>
        {
            private readonly IProgramRepository programRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListProgramByCohortContactViewModel>> Handle(ListProgramByCohortContactQuery request, CancellationToken cancellationToken)
            {
                var programs = await programRepository.ListProgramsByCohortContactAsync(request.UserId, cancellationToken);

                return mapper.Map<IEnumerable<ListProgramByCohortContactViewModel>>(programs).ToList();
            }
        }
        #endregion
    }
}
