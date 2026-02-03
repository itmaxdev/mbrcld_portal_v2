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
    internal sealed class LanguageSkillRepository : ILanguageSkillRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public LanguageSkillRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(LanguageSkill languageSkill, CancellationToken cancellationToken = default)
        {
            var odataTraining = this.mapper.Map<ODataLanguageSkill>(languageSkill);

            await webApiClient.For<ODataLanguageSkill>()
                 .Set(odataTraining)
                 .InsertEntryAsync(false, cancellationToken)
                 .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(LanguageSkill languageSkill, CancellationToken cancellationToken = default)
        {
            var odataLanguageSkill = this.mapper.Map<ODataLanguageSkill>(languageSkill);

            await this.webApiClient.For<ODataLanguageSkill>()
                .Key(odataLanguageSkill)
                .Set(odataLanguageSkill)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataLanguageSkill>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<LanguageSkill>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataLanguageSkill = await webApiClient.For<ODataLanguageSkill>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<LanguageSkill>(odataLanguageSkill);
        }

        public async Task<IList<LanguageSkill>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataLanguageSkills = await this.webApiClient.For<ODataLanguageSkill>()
                .Filter(c => c.ContactId.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync();

            return this.mapper.Map<IList<LanguageSkill>>(odataLanguageSkills);
        }
    }
}
