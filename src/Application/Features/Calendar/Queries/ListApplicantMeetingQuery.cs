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
    public sealed class ListApplicantMeetingQuery : IRequest<IList<ListApplicantMeetingViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListApplicantMeetingQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListApplicantMeetingQuery, IList<ListApplicantMeetingViewModel>>
        {
            private readonly ICalendarRepository calendarRepository;
            private readonly IMapper mapper;

            public QueryHandler(ICalendarRepository calendarRepository, IMapper mapper)
            {
                this.calendarRepository = calendarRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListApplicantMeetingViewModel>> Handle(ListApplicantMeetingQuery request, CancellationToken cancellationToken)
            {
                var userCalendar = await calendarRepository.ListApplicantMeetingAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<ListApplicantMeetingViewModel>>(userCalendar).ToList();
            }
        }
        #endregion
    }
}
