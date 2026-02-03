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
    public sealed class GetMaterialByIdQuery : IRequest<GetMaterialByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetMaterialByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetMaterialByIdQuery, GetMaterialByIdViewModel>
        {
            private readonly IMaterialRepository materialRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMaterialRepository materialRepository, IMapper mapper)
            {
                this.materialRepository = materialRepository;
                this.mapper = mapper;
            }

            public async Task<GetMaterialByIdViewModel> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
            {
                var material = await materialRepository.GetMaterialByIdAsync(request.Id, cancellationToken);
                return mapper.Map<GetMaterialByIdViewModel>(material);
            }
        }
        #endregion
    }
}
