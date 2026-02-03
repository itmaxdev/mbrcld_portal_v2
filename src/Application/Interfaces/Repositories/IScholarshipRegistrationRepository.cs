using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IScholarshipRegistrationRepository
    {
        Task<Result> CreateAsync(ScholarshipRegistration scholarshipRegistration, CancellationToken cancellationToken = default);
        Task<Maybe<ScholarshipRegistration>> GetScholarshipRegistrationByUserIdAndScholarshipIdAsync(Guid userId, Guid eventid, CancellationToken cancellationToken = default);
        Task<int> ListScholarshipRegistrationByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ScholarshipRegistration>> ScholarshipRegistrationByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    }
}
