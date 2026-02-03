using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<Result> CreateAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
        Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<Enrollment>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Enrollment>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<int> GetILPEnrollments( CancellationToken cancellationToken = default);
        Task<int> GetAllEnrollments( CancellationToken cancellationToken = default);
        Task<Maybe<Enrollment>> GetEnrollmentAsync(Guid programId, Guid userId, Guid CohortId, CancellationToken cancellationToken = default);
    }
}
