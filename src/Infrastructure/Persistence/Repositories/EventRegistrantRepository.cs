using AutoMapper;
using AutoMapper.Internal;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class EventRegistrantRepository : IEventRegistrantRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EventRegistrantRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(EventRegistrant eventRegistrant, CancellationToken cancellationToken = default)
        {
            var odataEventRegistrant = this.mapper.Map<ODataEventRegistrant>(eventRegistrant);

            await webApiClient.For<ODataEventRegistrant>()
                .Set(odataEventRegistrant)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task<Maybe<EventRegistrant>> GetEventRegistrantByUserIdAndEventIdAsync(Guid id, Guid eventid, CancellationToken cancellationToken = default)
        {
            var odataEventRegistrant = await webApiClient.For<ODataEventRegistrant>()
                .Filter(c => c.ContactId.ContactId == id)
                .Filter(e => e.EventId.EventId == eventid)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<EventRegistrant>(odataEventRegistrant);
        }

        public async Task<int> ListEventRegistrantByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var EventRegistrants = await webApiClient.For<ODataEventRegistrant>()
            .Filter(p => p.ContactId.ContactId == userId)
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

            return EventRegistrants.Count();
        }

        public async Task<IEnumerable<EventRegistrant>> EventRegistrantByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataEventRegistrant = await webApiClient.For<ODataEventRegistrantWithStatus>()
            .Filter(p => p.ContactId.ContactId == userId)
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

            return odataEventRegistrant.Select (item => this.mapper.Map<EventRegistrant>(item) );
        }
    }
}