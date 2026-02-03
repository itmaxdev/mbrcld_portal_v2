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
    internal sealed class EliteMentorSessionRepository : IEliteMentorSessionRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EliteMentorSessionRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<EliteMentorSession>> ListEliteMentorSessionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataEliteMentorSession = await this.webApiClient.For<ODataEliteMentorSession>()
                .Filter(c => c.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<EliteMentorSession>>(odataEliteMentorSession);
        }
        
        public async Task UpdateAsync(EliteMentorSession eliteMentorSession, CancellationToken cancellationToken = default)
        {
            var odataEliteMentorSession = this.mapper.Map<ODataEliteMentorSession>(eliteMentorSession);

            await this.webApiClient.For<ODataEliteMentorSession>()
                .Key(odataEliteMentorSession)
                .Set(odataEliteMentorSession)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<EliteMentorSession>> GetEliteMentorSessionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataEliteMentorSession = await webApiClient.For<ODataEliteMentorSession>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<EliteMentorSession>(odataEliteMentorSession);
        }
    }
}
