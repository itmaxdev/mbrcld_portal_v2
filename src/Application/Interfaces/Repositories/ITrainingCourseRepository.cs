using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ITrainingCourseRepository
    {
        Task<Result> CreateAsync(TrainingCourse trainingCourse, CancellationToken cancellationToken = default);
        Task UpdateAsync(TrainingCourse trainingCourse, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Maybe<TrainingCourse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IList<TrainingCourse>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
