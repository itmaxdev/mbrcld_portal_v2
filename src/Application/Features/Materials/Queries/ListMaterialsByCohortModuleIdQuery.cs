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
    public sealed class ListMaterialsByCohortModuleIdQuery : IRequest<IList<ListMaterialsByCohortModuleIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListMaterialsByCohortModuleIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListMaterialsByCohortModuleIdQuery, IList<ListMaterialsByCohortModuleIdViewModel>>
        {
            private readonly IMaterialRepository materialRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMaterialRepository materialRepository, IMapper mapper)
            {
                this.materialRepository = materialRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListMaterialsByCohortModuleIdViewModel>> Handle(ListMaterialsByCohortModuleIdQuery request, CancellationToken cancellationToken)
            {
                var materials = await materialRepository.ListMaterialsByCohortModuleIdAsync(request.Id, cancellationToken);
                return mapper.Map<IEnumerable<ListMaterialsByCohortModuleIdViewModel>>(materials).ToList();
            }
        }
        #endregion
    }
}
