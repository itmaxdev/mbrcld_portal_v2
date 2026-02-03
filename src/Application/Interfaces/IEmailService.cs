using Mbrcld.SharedKernel.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces
{
    public interface IEmailService
    {
        Task<Result> SendEmailConfirmationAsync(Guid userId, string link, CancellationToken cancellationToken = default);
        Task<Result> SendPasswordResetAsync(Guid userId, string link, CancellationToken cancellationToken = default);
        Task<Result> SendChangeEmailConfirmationAsync(Guid userId, string newEmail, string link, CancellationToken cancellation = default);
    }
}
