using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEliteClubRepository
    {
        Task<Maybe<EliteClub>> GetAlumniEliteClubAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Maybe<EliteClub>> GetEliteClubByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
