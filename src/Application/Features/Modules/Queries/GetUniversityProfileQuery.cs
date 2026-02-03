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
    public sealed class GetUniversityProfileQuery : IRequest<GetUniversityProfileViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetUniversityProfileQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetUniversityProfileQuery, GetUniversityProfileViewModel>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.mapper = mapper;
            }

            public async Task<GetUniversityProfileViewModel> Handle(GetUniversityProfileQuery request, CancellationToken cancellationToken)
            {
                var university = await moduleRepository.GetUniversityProfileAsync(request.Id, cancellationToken);
                return mapper.Map<GetUniversityProfileViewModel>(university);

            }
        }
        #endregion
    }
}
