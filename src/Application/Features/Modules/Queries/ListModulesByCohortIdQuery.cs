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
    public sealed class ListModulesByCohortIdQuery : IRequest<IList<ListModulesByCohortIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListModulesByCohortIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListModulesByCohortIdQuery, IList<ListModulesByCohortIdViewModel>>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListModulesByCohortIdViewModel>> Handle(ListModulesByCohortIdQuery request, CancellationToken cancellationToken)
            {
                var modules = await moduleRepository.ListModulesByCohortIdAsync(request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListModulesByCohortIdViewModel>>(modules).ToList();
            }
        }
        #endregion
    }
}
