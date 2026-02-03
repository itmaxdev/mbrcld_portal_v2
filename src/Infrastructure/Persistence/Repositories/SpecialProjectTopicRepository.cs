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
    internal sealed class SpecialProjectTopicRepository : ISpecialProjectTopicRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public SpecialProjectTopicRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<SpecialProjectTopic>> ListSpecialProjectTopicsAsync(CancellationToken cancellationToken = default)
        {
            var odataSpecialProjectTopics = await this.webApiClient.For<ODataSpecialProjectTopic>()
            .Filter(s => s.status == 1) //Active
            .OrderBy(x => x.SpecialProjectTopicName)
            .ProjectToModel()
            .FindEntriesAsync()
            .ConfigureAwait(false);

            return this.mapper.Map<IList<SpecialProjectTopic>>(odataSpecialProjectTopics);
        }
    }
}
