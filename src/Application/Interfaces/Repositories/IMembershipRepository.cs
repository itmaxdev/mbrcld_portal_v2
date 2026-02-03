using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IMembershipRepository
    {
        Task<Result> CreateAsync(Membership Membership, CancellationToken cancellationToken = default);
        Task UpdateAsync(Membership Membership, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<Membership>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Membership>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
