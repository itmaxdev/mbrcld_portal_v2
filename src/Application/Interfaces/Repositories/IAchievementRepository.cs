using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IAchievementRepository
    {
        Task<Result> CreateAsync(Achievement Achievement, CancellationToken cancellationToken = default);
        Task UpdateAsync(Achievement Achievement, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<Achievement>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Achievement>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
