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
    public sealed class GetUserCalendarQuery : IRequest<IList<GetUserCalendarViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public GetUserCalendarQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetUserCalendarQuery, IList<GetUserCalendarViewModel>>
        {
            private readonly ICalendarRepository calendarRepository;
            private readonly IMapper mapper;

            public QueryHandler(ICalendarRepository calendarRepository, IMapper mapper)
            {
                this.calendarRepository = calendarRepository;
                this.mapper = mapper;
            }

            public async Task<IList<GetUserCalendarViewModel>> Handle(GetUserCalendarQuery request, CancellationToken cancellationToken)
            {
                var userCalendar = await calendarRepository.GetUserCalendarAsync(request.UserId, cancellationToken);
                return mapper.Map<IEnumerable<GetUserCalendarViewModel>>(userCalendar).ToList();
            }
        }
        #endregion
    }
}
