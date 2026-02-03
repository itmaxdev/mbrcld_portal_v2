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
    public sealed class ListEventsQuery : IRequest<IList<ListEventsViewModel>>
    {
        public Guid UserId { get; }

        public ListEventsQuery(Guid userId)
        {
            UserId = userId;
        }
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListEventsQuery, IList<ListEventsViewModel>>
        {
            private readonly IEventRepository eventRepository;
            private readonly IMapper mapper;

           
            public QueryHandler(IEventRepository eventRepository, IMapper mapper)
            {
                this.eventRepository = eventRepository;
                this.mapper = mapper;

            }

            public async Task<IList<ListEventsViewModel>> Handle(ListEventsQuery request, CancellationToken cancellationToken)
            {
                var events = await eventRepository.GetEventsAsync(request.UserId,cancellationToken);
                return mapper.Map<IEnumerable<ListEventsViewModel>>(events).ToList();
            }
        }
        #endregion
    }
}
