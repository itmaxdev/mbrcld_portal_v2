using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEliteMembershipAttendanceRepository
    {
        Task<IList<EliteMembershipAttendance>> GetEliteMembershipAttendanceAsync(Guid userId, Guid eliteClubId, int membershipType, CancellationToken cancellationToken = default);
        Task<Maybe<EliteMembershipAttendance>> GetEliteMembershipAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(EliteMembershipAttendance eliteMembershipAttendance, CancellationToken cancellationToken = default);
    }
}
