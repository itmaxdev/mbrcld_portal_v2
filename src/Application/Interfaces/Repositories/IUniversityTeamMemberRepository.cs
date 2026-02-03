using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IUniversityTeamMemberRepository
    {
        Task<IList<UniversityTeamMember>> ListTeamMembersAsync(Guid userId ,CancellationToken cancellationToken = default);
        Task<IList<UniversityTeamMember>> ListTeamMembersByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default);
        Task<Maybe<UniversityTeamMember>> GetTeamMemberByIdAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Maybe<UniversityTeamMember>> CreateAsync(UniversityTeamMember teammember, CancellationToken cancellationToken = default);
        Task UpdateAsync(UniversityTeamMember teamMember, CancellationToken cancellationToken = default);
    }
}
