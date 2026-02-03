using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProgramAnswers.Queries
{
    public sealed class ListProgramAnswersByEnrollmentIdQuery : IRequest<IList<ListProgramAnswersByEnrollmentIdViewModel>>
    {
        #region Query
        public Guid EnrollmentId { get; }

        public ListProgramAnswersByEnrollmentIdQuery(Guid enrollmentId)
        {
            this.EnrollmentId = enrollmentId;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListProgramAnswersByEnrollmentIdQuery, IList<ListProgramAnswersByEnrollmentIdViewModel>>
        {
            private readonly IProgramAnswerRepository programAnswerRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramAnswerRepository programAnswerRepository, IMapper mapper)
            {
                this.programAnswerRepository = programAnswerRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListProgramAnswersByEnrollmentIdViewModel>> Handle(ListProgramAnswersByEnrollmentIdQuery request, CancellationToken cancellationToken)
            {
                var enrollments = await programAnswerRepository.ListByEnrollmentIdAsync(request.EnrollmentId).ConfigureAwait(false);
                return mapper.Map<IList<ListProgramAnswersByEnrollmentIdViewModel>>(enrollments);
            }
        }
        #endregion
    }
}
