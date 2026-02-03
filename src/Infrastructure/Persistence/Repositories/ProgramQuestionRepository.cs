using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class ProgramQuestionRepository : IProgramQuestionRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly IPreferredLanguageService preferredLanguageService;

        public ProgramQuestionRepository(
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

        public Task<IList<ProgramQuestion>> ListByProgramIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var lcid = this.preferredLanguageService.GetPreferredLanguageLCID();
            var cacheKey = $"{lcid}:{id}";

            return this.cache.GetOrCreateAsync(cacheKey, async (e) =>
            {
                e.AbsoluteExpiration = DateTime.UtcNow.AddHours(1);

                var odataProgramQuestions = (await webApiClient.For<ODataProgramQuestion>()
                    .Filter(c => c.CohortId.Id == id)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false))
                    .ToList();

                if (this.preferredLanguageService.GetPreferredLanguageLCID() == 1025)
                {
                    foreach (var question in odataProgramQuestions)
                    {
                        question.Name = question.Name_AR;
                    }
                }

                return this.mapper.Map<IList<ProgramQuestion>>(odataProgramQuestions);
            });
        }
    }
}
