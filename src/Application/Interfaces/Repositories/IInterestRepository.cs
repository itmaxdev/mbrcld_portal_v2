using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IInterestRepository
    {
        Task<Result> CreateAsync(Interest interest, CancellationToken cancellationToken = default);
        Task UpdateAsync(Interest interest, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<Interest>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Interest>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
