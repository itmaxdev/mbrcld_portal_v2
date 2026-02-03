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
    internal sealed class ScholarshipRegistrationRepository : IScholarshipRegistrationRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ScholarshipRegistrationRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(ScholarshipRegistration scholarshipRegistration, CancellationToken cancellationToken = default)
        {
            var odataScholarshipRegistration = this.mapper.Map<ODataScholarshipRegistration>(scholarshipRegistration);

            await webApiClient.For<ODataScholarshipRegistration>()
                .Set(odataScholarshipRegistration)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task<Maybe<ScholarshipRegistration>> GetScholarshipRegistrationByUserIdAndScholarshipIdAsync(Guid id, Guid scholarshipid, CancellationToken cancellationToken = default)
        {
            var odataScholarshipRegistration = await webApiClient.For<ODataScholarshipRegistration>()
                .Filter(c => c.ContactId.ContactId == id)
                .Filter(e => e.ScholarshipId.ScholarshipId == scholarshipid)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<ScholarshipRegistration>(odataScholarshipRegistration);
        }

        public async Task<int> ListScholarshipRegistrationByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var ScholarshipRegistration = await webApiClient.For<ODataScholarshipRegistration>()
            .Filter(p => p.ContactId.ContactId == userId)
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

            return ScholarshipRegistration.Count();
        }

        public async Task<IEnumerable<ScholarshipRegistration>> ScholarshipRegistrationByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataScholarshipRegistration = await webApiClient.For<ODataScholarshipRegistration>()
            .Filter(p => p.ContactId.ContactId == userId)
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

            return odataScholarshipRegistration.Select(item => this.mapper.Map<ScholarshipRegistration>(item));
        }
    }
}