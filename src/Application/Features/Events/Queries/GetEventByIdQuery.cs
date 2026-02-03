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
    public sealed class GetEventByIdQuery : IRequest<GetEventByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetEventByIdQuery(Guid id, Guid userid)
        {
            Id = id;
            UserId = userid;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetEventByIdQuery, GetEventByIdViewModel>
        {
            private readonly IEventRepository eventRepository;
            private readonly IEventRegistrantRepository eventregistrantRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEventRepository eventRepository, IEventRegistrantRepository eventregistrantRepository, IMapper mapper)
            {
                this.eventRepository = eventRepository;
                this.eventregistrantRepository = eventregistrantRepository;
                this.mapper = mapper;
            }

            public async Task<GetEventByIdViewModel> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
            {
                var events = await eventRepository.GetEventByIdAsync(request.Id).ConfigureAwait(false);
                if (events.HasValue)
                {
                    var canRegister = await eventRepository.GetEventAvailabilityAsync(request.Id).ConfigureAwait(false);
                    if(canRegister == false)
                    {
                        events.Value.AlreadyRegistered = true;
                    }
                    else
                    {
                        var eventregistrant = await eventregistrantRepository.GetEventRegistrantByUserIdAndEventIdAsync(request.UserId, request.Id).ConfigureAwait(false);
                        if (eventregistrant.HasValue)
                        {
                            events.Value.AlreadyRegistered = true;
                        }
                    }
                    return mapper.Map<GetEventByIdViewModel>(events.ValueOrDefault);
                }
                return null;
            }
        }
        #endregion
    }
}
