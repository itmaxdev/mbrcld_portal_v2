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
    internal sealed class EliteClubMemberRepository : IEliteClubMemberRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EliteClubMemberRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<EliteClubMember>> ListEliteClubMembersByIdAsync(Guid eliteClubId, CancellationToken cancellationToken = default)
        {
            var odataEliteClibMembers = await this.webApiClient.For<ODataEliteClubMember>()
                .Filter(c => c.EliteClub.Id == eliteClubId)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<EliteClubMember>>(odataEliteClibMembers);
        }
    }
}
