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
    internal sealed class ProfessionalExperienceRepository : IProfessionalExperienceRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ProfessionalExperienceRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(ProfessionalExperience professionalExperience, CancellationToken cancellationToken = default)
        {
            var odataProfExp = this.mapper.Map<ODataProfessionalExperience>(professionalExperience);

            await webApiClient.For<ODataProfessionalExperience>()
                .Set(odataProfExp)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(ProfessionalExperience professionalExperience, CancellationToken cancellationToken = default)
        {
            var odataProfExp = this.mapper.Map<ODataProfessionalExperience>(professionalExperience);

            await webApiClient.For<ODataProfessionalExperience>()
                .Key(odataProfExp)
                .Set(odataProfExp)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await webApiClient.For<ODataProfessionalExperience>()
               .Key(id)
               .DeleteEntryAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<Maybe<ProfessionalExperience>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataProfExp = await webApiClient.For<ODataProfessionalExperience>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<ProfessionalExperience>(odataProfExp);
        }

        public async Task<IList<ProfessionalExperience>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProfExp = await webApiClient.For<ODataProfessionalExperience>()
                .Filter(c => c.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<ProfessionalExperience>>(odataProfExp);
        }

        public async Task<ProfessionalExperience> ApplicantLatestProfessionalExperienceAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProfExp = await webApiClient.For<ODataProfessionalExperience>()
                .Filter(c => c.Contact.ContactId == userId)
                .Filter(c => c.To == null)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<ProfessionalExperience>(odataProfExp);
        }
    }
}
