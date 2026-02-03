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
    internal sealed class MembershipRepository : IMembershipRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public MembershipRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> CreateAsync(Membership Membership, CancellationToken cancellationToken = default)
        {
            var odataTraining = this.mapper.Map<ODataMembership>(Membership);

            await webApiClient.For<ODataMembership>()
                 .Set(odataTraining)
                 .InsertEntryAsync(false, cancellationToken)
                 .ConfigureAwait(false);

            return Result.Success();
        }

        public async Task UpdateAsync(Membership Membership, CancellationToken cancellationToken = default)
        {
            var odataMembership = this.mapper.Map<ODataMembership>(Membership);

            await this.webApiClient.For<ODataMembership>()
                .Key(odataMembership)
                .Set(odataMembership)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataMembership>()
                .Key(id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<Membership>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataMembership = await webApiClient.For<ODataMembership>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Membership>(odataMembership);
        }

        public async Task<IList<Membership>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataMemberships = await this.webApiClient.For<ODataMembership>()
                .Filter(c => c.ContactId.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Membership>>(odataMemberships);
        }
    }
}
