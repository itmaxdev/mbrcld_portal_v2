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
    public sealed class GetProgramDetailsByIdQuery : IRequest<GetProgramDetailsByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetProgramDetailsByIdQuery(Guid id, Guid userid)
        {
            Id = id;
            UserId = userid;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetProgramDetailsByIdQuery, GetProgramDetailsByIdViewModel>
        {
            private readonly IProgramRepository programRepository;
            private readonly IEnrollmentRepository enrollmentRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IEnrollmentRepository enrollmentRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.enrollmentRepository = enrollmentRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<GetProgramDetailsByIdViewModel> Handle(GetProgramDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                var program = await programRepository.GetProgramDetailsByIdAsync(request.Id, request.UserId, cancellationToken);
                var programDetails = mapper.Map<GetProgramDetailsByIdViewModel>(program);
                if (program.LastEnrollmentDate < DateTime.Now)
                {
                    programDetails.EnrollmentStatus = "Closed";
                }
                else
                {
                    var enrollment = await enrollmentRepository.GetEnrollmentAsync(request.Id, request.UserId, program.CohortId, cancellationToken);
                    if (enrollment.HasValue)
                    {
                        programDetails.EnrollmentId = enrollment.Value.Id;
                        programDetails.EnrollmentStatus = enrollment.Value.Stage == 0 ? "applied" : "enrolled";
                    }
                }
                return programDetails;
            }
        }
        #endregion
    }
}
