using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Queries
{
    public sealed class GetEnrollmentByIdQuery : IRequest<GetEnrollmentByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetEnrollmentByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetEnrollmentByIdQuery, GetEnrollmentByIdViewModel>
        {
            private readonly IEnrollmentRepository enrollmentRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEnrollmentRepository enrollmentRepository, IMapper mapper)
            {
                this.enrollmentRepository = enrollmentRepository;
                this.mapper = mapper;
            }

            public async Task<GetEnrollmentByIdViewModel> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
            {
                var enrollments = await enrollmentRepository.GetByIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<GetEnrollmentByIdViewModel>(enrollments.ValueOrDefault);
            }
        }
        #endregion
    }
}
