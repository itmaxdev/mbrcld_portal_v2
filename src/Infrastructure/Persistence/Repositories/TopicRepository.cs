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
    internal sealed class TopicRepository : ITopicRepository
    {
        private readonly IProgramRepository programRepository;
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public TopicRepository(
            IProgramRepository programRepository,
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.programRepository = programRepository;
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<Topic>> ListTopicsByProgramIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var program = await this.webApiClient.For<ODataProgram>()
                .Filter(c => c.Id == id)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            if (program.Any())
            {
                var cohortId = program.FirstOrDefault().ActiveCohort.Id;

                var odataTopics = await this.webApiClient.For<ODataTopic>()
                .Filter(c => c.Cohort.Id == cohortId)
                .OrderByDescending(x => x.TopicName)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

                return this.mapper.Map<IList<Topic>>(odataTopics);
            }
            return null;
        }

        public async Task<Maybe<Topic>> GetTopicByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataTopic = await webApiClient.For<ODataTopic>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Topic>(odataTopic);
        }
    }
}
