using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Persistence.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Mbrcld.Infrastructure.Extensions;
using System.Linq;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class EventQuestionRepository : IEventQuestionRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly IPreferredLanguageService preferredLanguageService;

        public EventQuestionRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper,
            IMemoryCache cache,
            IPreferredLanguageService preferredLanguageService)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.cache = cache;
            this.preferredLanguageService = preferredLanguageService;
        }

        public Task<IList<EventQuestion>> ListByEventIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var lcid = this.preferredLanguageService.GetPreferredLanguageLCID();
            var cacheKey = $"{lcid}:{id}";

            return this.cache.GetOrCreateAsync(cacheKey, async (e) =>
            {
                e.AbsoluteExpiration = DateTime.UtcNow.AddHours(1);

                var odataEventQuestions = (await webApiClient.For<ODataEventQuestion>()
                    .Filter(e => e.EventId.EventId == id)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false))
                    .ToList();

                if (this.preferredLanguageService.GetPreferredLanguageLCID() == 1025)
                {
                    foreach (var question in odataEventQuestions)
                    {
                        question.NameTest = question.Name_AR;
                    }
                }

                return this.mapper.Map<IList<EventQuestion>>(odataEventQuestions);
            });
        }
    }
}
