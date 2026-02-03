using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEducationQualificationRepository
    {
        Task<Result> CreateAsync(EducationQualification educationQualification, CancellationToken cancellationToken = default);
        Task UpdateAsync(EducationQualification educationQualification, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<EducationQualification>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<EducationQualification>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<EducationQualification> ApplicantLatestEducationAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
