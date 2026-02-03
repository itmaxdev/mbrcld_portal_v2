using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.EducationQualifications.Queries
{
    public sealed class ListUserEducationQualificationsQuery : IRequest<IList<ListUserEducationQualificationsViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListUserEducationQualificationsQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserEducationQualificationsQuery, IList<ListUserEducationQualificationsViewModel>>
        {
            private readonly IEducationQualificationRepository educationQualificationRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEducationQualificationRepository educationQualificationRepository, IMapper mapper)
            {
                this.educationQualificationRepository = educationQualificationRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserEducationQualificationsViewModel>> Handle(ListUserEducationQualificationsQuery request, CancellationToken cancellationToken)
            {
                var qualifications = await educationQualificationRepository.ListByUserIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return mapper.Map<IList<ListUserEducationQualificationsViewModel>>(qualifications);
            }
        }
        #endregion
    }
}
