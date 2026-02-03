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
    internal sealed class UniversityTeamMemberRepository : IUniversityTeamMemberRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IUserRepository userRepository;
        private readonly IModuleRepository moduleRepository;
        private readonly IMapper mapper;

        public UniversityTeamMemberRepository(
            ISimpleWebApiClient webApiClient,
            IUserRepository userRepository,
            IModuleRepository moduleRepository,
            IMapper mapper
            )
        {
            this.webApiClient = webApiClient;
            this.userRepository = userRepository;
            this.moduleRepository = moduleRepository;
            this.mapper = mapper;
        }

        public async Task<IList<UniversityTeamMember>> ListTeamMembersAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId);
            var university = user.Value.UniversityId;
            var odataTeamMembers = await this.webApiClient.For<ODataUniversityTeamMember>()
                .Filter(c => c.University.UniversityId == university)
                .Filter(c => c.Status == 0)//Active
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<UniversityTeamMember>>(odataTeamMembers);
        }

        public async Task<IList<UniversityTeamMember>> ListTeamMembersByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default)
        {
            var module = await moduleRepository.GetModuleByIdAsync(moduleId);
            var instructor = await userRepository.GetByIdAsync(module.InstructorId);
            var university = instructor.Value.UniversityId;
            var odataTeamMembers = await this.webApiClient.For<ODataUniversityTeamMember>()
                .Filter(c => c.University.UniversityId == university)
                .Filter(c => c.Status == 0)//Active
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<UniversityTeamMember>>(odataTeamMembers);
        }

        public async Task<Maybe<UniversityTeamMember>> GetTeamMemberByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataUniversityTeamMember = await webApiClient.For<ODataUniversityTeamMember>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<UniversityTeamMember>(odataUniversityTeamMember);
        }

        public async Task<Maybe<UniversityTeamMember>> CreateAsync(UniversityTeamMember teammember, CancellationToken cancellationToken = default)
        {
            var odataTeamMember = this.mapper.Map<ODataUniversityTeamMember>(teammember);

            await webApiClient.For<ODataUniversityTeamMember>()
                .Set(odataTeamMember)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<UniversityTeamMember>(odataTeamMember);
        }

        public async Task UpdateAsync(UniversityTeamMember TeamMember, CancellationToken cancellationToken = default)
        {
            var odataTeamMember = this.mapper.Map<ODataUniversityTeamMember>(TeamMember);

            await this.webApiClient.For<ODataUniversityTeamMember>()
                .Key(odataTeamMember)
                .Set(odataTeamMember)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
