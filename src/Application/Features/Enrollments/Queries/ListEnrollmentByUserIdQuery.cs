using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Queries
{
    public sealed class ListEnrollmentByUserIdQuery : IRequest<IList<ListEnrollmentByUserIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListEnrollmentByUserIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListEnrollmentByUserIdQuery, IList<ListEnrollmentByUserIdViewModel>>
        {
            private readonly IEnrollmentRepository enrollmentRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEnrollmentRepository enrollmentRepository, IMapper mapper)
            {
                this.enrollmentRepository = enrollmentRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListEnrollmentByUserIdViewModel>> Handle(ListEnrollmentByUserIdQuery request, CancellationToken cancellationToken)
            {
                var enrollments = await enrollmentRepository.ListByUserIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IList<ListEnrollmentByUserIdViewModel>>(enrollments);
            }
        }
        #endregion
    }
}