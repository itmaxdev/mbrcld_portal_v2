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
    public sealed class GetCurrentModuleQuery : IRequest<GetCurrentModuleViewModel>
    {
        #region Query
        public Guid UserId { get; }


        public GetCurrentModuleQuery(Guid userId)
        {
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetCurrentModuleQuery, GetCurrentModuleViewModel>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.mapper = mapper;
            }

            public async Task<GetCurrentModuleViewModel> Handle(GetCurrentModuleQuery request, CancellationToken cancellationToken)
            {
                var module = await moduleRepository.GetCurrentModuleAsync(request.UserId, cancellationToken);

                return mapper.Map<GetCurrentModuleViewModel>(module);
            }
        }
        #endregion
    }
}
