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
    internal sealed class InterestRepository : IInterestRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public InterestRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(Interest intereset, CancellationToken cancellationToken = default)
        {
            var odataInterest = this.mapper.Map<ODataSkillAndInterest>(intereset);

            await webApiClient.For<ODataSkillAndInterest>()
                 .Set(odataInterest)
                 .InsertEntryAsync(false, cancellationToken)
                 .ConfigureAwait(false);

            return Result.Success(intereset);
        }

        public async Task UpdateAsync(Interest interest, CancellationToken cancellationToken = default)
        {
            var odataInterest = this.mapper.Map<ODataSkillAndInterest>(interest);

            await this.webApiClient.For<ODataSkillAndInterest>()
                .Key(odataInterest)
                .Set(odataInterest)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataSkillAndInterest>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<Interest>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataSkill_Interest = await webApiClient.For<ODataSkillAndInterest>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Interest>(odataSkill_Interest);
        }

        public async Task<IList<Interest>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataInterests = await this.webApiClient.For<ODataSkillAndInterest>()
                .Filter(c => c.ContactId.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Interest>>(odataInterests);
        }
    }
}
