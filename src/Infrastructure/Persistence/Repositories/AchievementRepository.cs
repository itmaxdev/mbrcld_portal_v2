using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class AchievementRepository : IAchievementRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public AchievementRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(Achievement achievement, CancellationToken cancellationToken = default)
        {
            var odataTraining = this.mapper.Map<ODataAchievement>(achievement);

            await webApiClient.For<ODataAchievement>()
                 .Set(odataTraining)
                 .InsertEntryAsync(false, cancellationToken)
                 .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(Achievement Achievement, CancellationToken cancellationToken = default)
        {
            var odataAchievement = this.mapper.Map<ODataAchievement>(Achievement);

            await this.webApiClient.For<ODataAchievement>()
                .Key(odataAchievement)
                .Set(odataAchievement)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataAchievement>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<Achievement>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataAchievement = await webApiClient.For<ODataAchievement>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Achievement>(odataAchievement);
        }

        public async Task<IList<Achievement>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataAchievements = await this.webApiClient.For<ODataAchievement>()
                .Filter(c => c.ContactId.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Achievement>>(odataAchievements);
        }
    }
}
