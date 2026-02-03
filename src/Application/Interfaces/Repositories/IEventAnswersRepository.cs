using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IEventAnswersRepository
    {
        Task<Result> CreateAsync(EventAnswer eventanswers, CancellationToken cancellationToken = default);
    }
}
