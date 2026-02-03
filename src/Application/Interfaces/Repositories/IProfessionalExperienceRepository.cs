using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IProfessionalExperienceRepository
    {
        Task<Result> CreateAsync(ProfessionalExperience profExp, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProfessionalExperience profExp, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<ProfessionalExperience>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<ProfessionalExperience>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ProfessionalExperience> ApplicantLatestProfessionalExperienceAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
