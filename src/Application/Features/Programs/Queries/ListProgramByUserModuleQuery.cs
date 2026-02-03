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
    public sealed class ListProgramByUserModuleQuery : IRequest<IList<ListProgramByUserModuleViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListProgramByUserModuleQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListProgramByUserModuleQuery, IList<ListProgramByUserModuleViewModel>>
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

            public async Task<IList<ListProgramByUserModuleViewModel>> Handle(ListProgramByUserModuleQuery request, CancellationToken cancellationToken)
            {
                var programs = await programRepository.ListProgramsByUserModulesAsync(request.UserId, cancellationToken);

                return mapper.Map<IEnumerable<ListProgramByUserModuleViewModel>>(programs).ToList();
            }
        }
        #endregion
    }
}
