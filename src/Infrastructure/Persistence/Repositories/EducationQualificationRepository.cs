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
    internal sealed class EducationQualificationRepository : IEducationQualificationRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EducationQualificationRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(EducationQualification qualification, CancellationToken cancellationToken = default)
        {
            var odataQualification = this.mapper.Map<ODataEducationQualification>(qualification);

            await webApiClient.For<ODataEducationQualification>()
                .Set(odataQualification)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(EducationQualification qualification, CancellationToken cancellationToken = default)
        {
            var odataQualification = this.mapper.Map<ODataEducationQualification>(qualification);

            await webApiClient.For<ODataEducationQualification>()
                .Key(odataQualification)
                .Set(odataQualification)
                .UpdateEntryAsync(false, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await webApiClient.For<ODataEducationQualification>()
               .Key(id)
               .DeleteEntryAsync(cancellationToken)
               .ConfigureAwait(false);
        }

        public async Task<Maybe<EducationQualification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataQualification = await webApiClient.For<ODataEducationQualification>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<EducationQualification>(odataQualification);
        }

        public async Task<IList<EducationQualification>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataQualification = await webApiClient.For<ODataEducationQualification>()
                .Filter(c => c.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<EducationQualification>>(odataQualification);
        }

        public async Task<EducationQualification> ApplicantLatestEducationAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataQualification = await webApiClient.For<ODataEducationQualification>()
                .Filter(c => c.Contact.ContactId == userId)
                .OrderByDescending(c => c.Graduationdate)
                .Top(1)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<EducationQualification>(odataQualification);
        }
    }
}
