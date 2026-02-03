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
    internal sealed class ReferenceRepository : IReferenceRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public ReferenceRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(Reference reference, CancellationToken cancellationToken = default)
        {
            var odatareference = this.mapper.Map<ODataReference>(reference);

            await webApiClient.For<ODataReference>()
                .Set(odatareference)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(Reference reference, CancellationToken cancellationToken = default)
        {
            var odataReference = this.mapper.Map<ODataReference>(reference);

            await webApiClient.For<ODataReference>()
                .Key(odataReference)
                .Set(odataReference)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<Reference>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataReference = await webApiClient.For<ODataReference>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Reference>(odataReference);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await webApiClient.For<ODataReference>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IList<Reference>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataReferences = await webApiClient.For<ODataReference>()
                .Filter(c => c.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Reference>>(odataReferences);
        }
    }
}
