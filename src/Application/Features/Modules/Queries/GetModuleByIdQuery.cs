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
    public sealed class GetModuleByIdQuery : IRequest<GetModuleByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetModuleByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetModuleByIdQuery, GetModuleByIdViewModel>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.mapper = mapper;
            }

            public async Task<GetModuleByIdViewModel> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
            {
                var module = await moduleRepository.GetModuleByIdAsync(request.Id, cancellationToken);

                return mapper.Map<GetModuleByIdViewModel>(module);
            }
        }
        #endregion
    }
}
