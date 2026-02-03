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
    public sealed class SearchAlumniCriteriaQuery : IRequest<IList<SearchAlumniCriteriaViewModel>>
    {
        #region Query
        public Guid? ProgramId { get; }
        public Guid? SectorId { get; }
        public int? Year { get; }
        public Guid UserId { get; }

        public SearchAlumniCriteriaQuery(Guid? programId, Guid? sectorId, int? year, Guid userId)
        {
            ProgramId = programId;
            SectorId = sectorId;
            Year = year;
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<SearchAlumniCriteriaQuery, IList<SearchAlumniCriteriaViewModel>>
        {
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<IList<SearchAlumniCriteriaViewModel>> Handle(SearchAlumniCriteriaQuery request, CancellationToken cancellationToken)
            {
                var alumnies = await userRepository.SearchAlumniCriteriaAsync(request.ProgramId, request.SectorId, request.Year, request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<SearchAlumniCriteriaViewModel>>(alumnies).ToList();
            }
        }
        #endregion
    }
}
