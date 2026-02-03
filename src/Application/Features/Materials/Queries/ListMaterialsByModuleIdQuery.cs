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
    public sealed class ListMaterialsByModuleIdQuery : IRequest<IList<ListMaterialsByModuleIdViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public ListMaterialsByModuleIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListMaterialsByModuleIdQuery, IList<ListMaterialsByModuleIdViewModel>>
        {
            private readonly IMaterialRepository materialRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMaterialRepository materialRepository, IMapper mapper)
            {
                this.materialRepository = materialRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListMaterialsByModuleIdViewModel>> Handle(ListMaterialsByModuleIdQuery request, CancellationToken cancellationToken)
            {
                var materials = await materialRepository.ListMaterialsByModuleIdAsync(request.Id, request.UserId, cancellationToken);

                return mapper.Map<IEnumerable<ListMaterialsByModuleIdViewModel>>(materials).ToList();
            }
        }
        #endregion
    }
}
