using AutoMapper;
using Mbrcld.Application.Features.ProfessionalExperiences.Queries;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Mbrcld.Domain.Entities;
using static Mbrcld.Domain.Entities.EventRegistrant;

namespace Mbrcld.Application.Features.Events.Queries
{
    public sealed class ListUserEventRegistrantQuery : IRequest<IList<ListUserEventRegistrantViewModel>>
    {
        #region Query
        public Guid Id { get; }
        //public EventRegistrantStatus StatusCode { get; set; }

        public ListUserEventRegistrantQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserEventRegistrantQuery, IList<ListUserEventRegistrantViewModel>>
        {
            private readonly IEventRegistrantRepository eventRegistrantRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEventRegistrantRepository eventRegistrantRepository, IMapper mapper)
            {
                this.eventRegistrantRepository = eventRegistrantRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserEventRegistrantViewModel>> Handle(ListUserEventRegistrantQuery request, CancellationToken cancellationToken)
            {
                var eventRegistrant = await eventRegistrantRepository.EventRegistrantByUserIdAsync(request.Id).ConfigureAwait(false);
                var eventRegistrantViewModels = eventRegistrant.Select(eventRegistrant => new ListUserEventRegistrantViewModel
                {
                    EventId = eventRegistrant.EventId,
                    Registrant = eventRegistrant.UserId,
                    Name = eventRegistrant.Name,
                    // StatusCode = eventRegistrant.StatusCode,
                    StatusCode = Enum.GetName(typeof(EventRegistrantStatus), eventRegistrant.StatusCode)
                }).ToList();
                return eventRegistrantViewModels;
            }
        }
        #endregion
    }
}