using AutoMapper;
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
    internal sealed class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EnrollmentRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
        {
            var odataEnrollment = this.mapper.Map<ODataEnrollment>(enrollment);

            await webApiClient.For<ODataEnrollment>()
                .Set(odataEnrollment)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
        {
            var odataEnrollment = this.mapper.Map<ODataEnrollment>(enrollment);

            await webApiClient.For<ODataEnrollment>()
                .Key(odataEnrollment)
                .Set(odataEnrollment)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<Enrollment>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataEnrollment = await webApiClient.For<ODataEnrollment>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Enrollment>(odataEnrollment);
        }
        public async Task<Maybe<Enrollment>> GetEnrollmentAsync(Guid programId, Guid userId, Guid cohortId, CancellationToken cancellationToken = default)
        {
            var odataEnrollment = await webApiClient.For<ODataEnrollment>()
                .Filter(c => c.Contact.ContactId == userId)
                .Filter(c => c.Program.Id == programId)
                .Filter(c => c.Cohort.Id == cohortId)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Enrollment>(odataEnrollment);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await webApiClient.For<ODataEnrollment>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IList<Enrollment>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataEnrollments = await webApiClient.For<ODataEnrollment>()
                .Filter(c => c.Contact.ContactId == userId)
               // .Filter(c=>c.Cohort.OpenForRegistration==true)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Enrollment>>(odataEnrollments);
        }

        public async Task<int> GetILPEnrollments(CancellationToken cancellationToken = default)
        {
            var odataEnrollments = await webApiClient.For<ODataEnrollment>()
                           .Filter(c => c.Cohort.Name.Contains("ILP"))
                           .ProjectToModel()
                           .FindEntriesAsync(cancellationToken)
                           .ConfigureAwait(false);
            return odataEnrollments.ToList().Count;
        }

        public async Task<int> GetAllEnrollments(CancellationToken cancellationToken = default)
        {
            var odataEnrollments = await webApiClient.For<ODataEnrollment>()
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return odataEnrollments.ToList().Count;
        }
    }
}