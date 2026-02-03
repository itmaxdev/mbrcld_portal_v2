using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class EventRepository : IEventRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IUserRepository user;


        public EventRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper, IUserRepository user
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.user = user;

        }

        public async Task<Maybe<Event>> GetEventByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataEvent = await webApiClient.For<ODataEvent>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Event>(odataEvent);
        }
        public async Task<bool> GetEventAvailabilityAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataEvent = await webApiClient.For<ODataEvent>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);
            
            if(odataEvent != null)
            {
                var maxCapacity = odataEvent.MaxCapacity;

                var odataEventRegistrants = await webApiClient.For<ODataEventRegistrant>()
                    .Filter(c => c.EventId.EventId == id)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);

                var registered = odataEventRegistrants.Count();

                if (registered < maxCapacity)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<IList<Event>> GetEventsAsync(Guid userId,CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);
            //User.Value.Role = 2;
            IEnumerable<ODataEvent> odataEvents;
            if ( User.Value.Role == 3) //Applicant or Alumni
            {
                odataEvents = await this.webApiClient.For<ODataEvent>()
                // .Filter(p=>p.)
                .OrderByDescending(x => x.FromDate)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);
            }
            else
            {
                odataEvents = await this.webApiClient.For<ODataEvent>()
                .Filter(p=>p.AlumniOnly==false)
                .OrderByDescending(x => x.FromDate)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);
            }
            return this.mapper.Map<IList<Event>>(odataEvents);
        }
    }
}
