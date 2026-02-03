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
    public sealed class GetUniversityByIdQuery : IRequest<GetUniversityByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetUniversityByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetUniversityByIdQuery, GetUniversityByIdViewModel>
        {
            private readonly IUniversityRepository universityRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUniversityRepository universityRepository, IMapper mapper)
            {
                this.universityRepository = universityRepository;
                this.mapper = mapper;
            }

            public async Task<GetUniversityByIdViewModel> Handle(GetUniversityByIdQuery request, CancellationToken cancellationToken)
            {
                var university = await universityRepository.GetUniversityByIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<GetUniversityByIdViewModel>(university.ValueOrDefault);
            }
        }
        #endregion
    }
}
