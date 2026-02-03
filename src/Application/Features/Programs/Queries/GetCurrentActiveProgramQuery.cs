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
    public sealed class GetCurrentActiveProgramQuery : IRequest<GetCurrentActiveProgramViewModel>
    {
        #region Query

        public GetCurrentActiveProgramQuery()
        {

        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetCurrentActiveProgramQuery, GetCurrentActiveProgramViewModel>
        {
            private readonly IProgramRepository programRepository;
            //private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                //this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<GetCurrentActiveProgramViewModel> Handle(GetCurrentActiveProgramQuery request, CancellationToken cancellationToken)
            {
                var program = await programRepository.GetCurrentActiveProgramAsync(cancellationToken);

                return mapper.Map<GetCurrentActiveProgramViewModel>(program);
            }
        }
        #endregion
    }
}
