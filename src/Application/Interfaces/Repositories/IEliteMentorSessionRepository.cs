using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEliteMentorSessionRepository
    {
        Task<IList<EliteMentorSession>> ListEliteMentorSessionByUserIdAsync(Guid userId ,CancellationToken cancellationToken = default);
        Task UpdateAsync(EliteMentorSession eliteMentorSession, CancellationToken cancellationToken = default);
        Task<Maybe<EliteMentorSession>> GetEliteMentorSessionByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    }
}
