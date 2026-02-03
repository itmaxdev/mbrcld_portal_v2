using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface ISurveyRepository
    {
        Task<IList<Survey>> ListUserSurveysUrlAsync(Guid userId ,CancellationToken cancellationToken = default);
        Task<IList<Survey>> ListSurveysByProgramIdAsync(Guid userId , CancellationToken cancellationToken = default);
        Task<IList<Survey>> ListUserCompletedSurveysAsync(Guid userId ,CancellationToken cancellationToken = default);
    }
}
