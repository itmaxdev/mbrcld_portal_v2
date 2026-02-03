using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IProgramAnswerRepository
    {
        Task<Result> CreateAsync(ProgramAnswer programAnswser, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProgramAnswer programAnswser, CancellationToken cancellationToken = default);
        Task<Maybe<ProgramAnswer>> GetAsync(Guid enrollmentId, Guid questionId, CancellationToken cancellationToken = default);
        Task<IList<ProgramAnswer>> ListByEnrollmentIdAsync(Guid programId, CancellationToken cancellationToken = default);
    }
}
