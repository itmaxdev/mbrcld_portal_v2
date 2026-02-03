using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IProgramQuestionRepository
    {
        Task<IList<ProgramQuestion>> ListByProgramIdAsync(Guid programId, CancellationToken cancellationToken = default);
    }
}
