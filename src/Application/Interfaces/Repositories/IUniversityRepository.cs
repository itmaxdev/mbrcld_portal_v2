using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IUniversityRepository
    {
        Task<Maybe<University>> GetUniversityByIdAsync(Guid userid, CancellationToken cancellationToken = default);
        Task<Maybe<University>> GetUniversityAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(University University, CancellationToken cancellationToken = default);
    }
}
