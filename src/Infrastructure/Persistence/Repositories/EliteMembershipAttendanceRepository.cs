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
    internal sealed class EliteMembershipAttendanceRepository : IEliteMembershipAttendanceRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public EliteMembershipAttendanceRepository(
            ISimpleWebApiClient webApiClient,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<IList<EliteMembershipAttendance>> GetEliteMembershipAttendanceAsync(Guid userId, Guid eliteClubId, int membershipType, CancellationToken cancellationToken = default)
        {
            List<ODataEliteMembershipAttendance> membershipAttendance = new List<ODataEliteMembershipAttendance> { };

            var eliteMemberships = await this.webApiClient.For<ODataEliteMembership>()
                .Filter(c => c.EliteClub.Id == eliteClubId)
                .Filter(c => c.Type == membershipType)
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            foreach(var eliteMembership in eliteMemberships) { 

                Guid eliteMembershipId = eliteMembership.Id;
                var eliteMembershipAttendances = await this.webApiClient.For<ODataEliteMembershipAttendance>()
               .Filter(c => c.Contact.ContactId == userId)
               .Filter(c => c.EliteMembership.Id == eliteMembershipId)
               .ProjectToModel()
               .FindEntriesAsync()
               .ConfigureAwait(false);

                foreach( var eliteMembershipAttendance in eliteMembershipAttendances)
                {
                    membershipAttendance.Add(eliteMembershipAttendance);
                }
            } 
            return this.mapper.Map<IList<EliteMembershipAttendance>>(membershipAttendance);
        }

        public async Task<Maybe<EliteMembershipAttendance>> GetEliteMembershipAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataEliteMembershipAttendance = await webApiClient.For<ODataEliteMembershipAttendance>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<EliteMembershipAttendance>(odataEliteMembershipAttendance);
        }

        public async Task UpdateAsync(EliteMembershipAttendance eliteMembershipAttendance, CancellationToken cancellationToken = default)
        {
            var odataEliteMembershipAttendance = this.mapper.Map<ODataEliteMembershipAttendance>(eliteMembershipAttendance);

            await this.webApiClient.For<ODataEliteMembershipAttendance>()
                .Key(odataEliteMembershipAttendance)
                .Set(odataEliteMembershipAttendance)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
