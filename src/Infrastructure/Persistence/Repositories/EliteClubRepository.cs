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
    internal sealed class EliteClubRepository : IEliteClubRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EliteClubRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<EliteClub>> GetAlumniEliteClubAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var eliteClubMembers = await this.webApiClient.For<ODataEliteClubMember>()
                .Filter(c => c.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            foreach (var eliteClubMember in eliteClubMembers)
            {
                var now = DateTime.Now;
                var eliteClubs = await this.webApiClient.For<ODataEliteClub>()
                .Filter(c => c.FromDate <= now && c.ToDate >= now)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

                if (eliteClubs.Any())
                {
                    return this.mapper.Map<EliteClub>(eliteClubs.FirstOrDefault());
                }
            }

            return null;
        }

        public async Task<Maybe<EliteClub>> GetEliteClubByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataEliteClub = await webApiClient.For<ODataEliteClub>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<EliteClub>(odataEliteClub);
        }
    }
}
