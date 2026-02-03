using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IReferenceRepository
    {
        Task<Result> CreateAsync(Reference reference, CancellationToken cancellationToken = default);
        Task UpdateAsync(Reference reference, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<Reference>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Reference>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
