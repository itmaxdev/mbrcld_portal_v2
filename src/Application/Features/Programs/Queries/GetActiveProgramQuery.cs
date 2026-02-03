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
    public sealed class GetActiveProgramQuery : IRequest<GetActiveProgramViewModel>
    {
        #region Query
        public Guid UserId { get; }

        public GetActiveProgramQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetActiveProgramQuery, GetActiveProgramViewModel>
        {
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.mapper = mapper;
            }

            public async Task<GetActiveProgramViewModel> Handle(GetActiveProgramQuery request, CancellationToken cancellationToken)
            {
                var program = await programRepository.GetActiveProgramAsync(request.UserId, cancellationToken);

                return mapper.Map<GetActiveProgramViewModel>(program);
            }
        }
        #endregion
    }
}
