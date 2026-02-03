using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IScholarshipRepository
    {
        Task<Maybe<Scholarship>> GetScholarshipByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<Scholarship>> GetScholarshipAsync(CancellationToken cancellationToken = default);
    }
}