using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Result> CreateAsync(User user, string password, CancellationToken cancellationToken = default);

        Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = default);

        Task<Maybe<User>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Maybe<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<Result> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default);

        Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

        Task<Result> SendChangeEmailConfirmationLinkAsync(Guid userId, string newEmail, CancellationToken cancellationToken = default);

        Task<Result> ChangeEmailAsync(string currentEmail, string newEmail, string token, CancellationToken cancellationToken = default);

        Task<Result> SendPasswordResetLinkAsync(string email, CancellationToken cancellationToken = default);

        Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);
        Task<Result> DeleteAccountAsync(string email, CancellationToken cancellationToken = default);

        Task<Result<bool>> VerifyPasswordResetTokenAsync(string email, string token, CancellationToken cancellationToken = default);
    }
}
