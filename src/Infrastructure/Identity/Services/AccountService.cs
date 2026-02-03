using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Identity.Services
{
    internal sealed class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly IUserRepository userRepository;
        private readonly IUrlHelperService urlHelper;
        private readonly IEmailService emailService;
        private readonly IdentityOptions options;

        public AccountService(
            UserManager<User> userManager,
            IUserRepository userRepository,
            IUrlHelperService urlHelper,
            IOptions<IdentityOptions> optionsAccessor,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.urlHelper = urlHelper;
            this.emailService = emailService;
            this.userRepository = userRepository;
            this.options = optionsAccessor?.Value ?? new IdentityOptions();
        }

        public async Task<Result> CreateAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            // Check emiratesId is exist
            var isEmirateIdExist = await userRepository.GetByEmiratesIdAsync(user.EmiratesId);
            if (isEmirateIdExist != null)
            {
                return Result.Failure("Emirate Id already exist.");
            }

            var result = await this.userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result.ToResult();
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var email = await this.userManager.GetEmailAsync(user);
            string emailConfirmationLink = this.urlHelper.GetAbsoluteUrlForAction("ConfirmEmail", new { token, email });

            await this.emailService.SendEmailConfirmationAsync(user.Id, emailConfirmationLink, cancellationToken);

            return Result.Success();
        }              

        public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var result = await this.userManager.UpdateAsync(user);
            return result.ToResult();
        }

        public async Task<Maybe<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await this.userManager.FindByIdAsync(id.ToString());
        }

        public async Task<Maybe<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await this.userManager.FindByEmailAsync(email);
        }

        public async Task<Result> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // TODO error description
                return Result.Failure();
            }

            var result = await this.userManager.ConfirmEmailAsync(user, token);
            return result.ToResult();
        }

        public async Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            var result = await this.userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.ToResult();
        }

        public async Task<Result> SendChangeEmailConfirmationLinkAsync(Guid userId, string newEmail, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                // TODO error description
                return Result.Failure();
            }

            var token = await this.userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var currentEmail = await this.userManager.GetEmailAsync(user);
            var link = this.urlHelper.GetAbsoluteUrlForAction("ChangeEmailConfirmation", new { token, currentEmail, newEmail });

            return await this.emailService.SendChangeEmailConfirmationAsync(userId, newEmail, link, cancellationToken);
        }

        public async Task<Result> ChangeEmailAsync(string currentEmail, string newEmail, string token, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(currentEmail);
            if (user == null)
            {
                // TODO error description
                return Result.Failure();
            }

            var result = await this.userManager.ChangeEmailAsync(user, newEmail, token);
            return result.ToResult();
        }

        public async Task<Result> SendPasswordResetLinkAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // TODO error description
                return Result.Failure();
            }

            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var link = this.urlHelper.GetAbsoluteUrlForAction("ResetPassword", new { token, email });

            return await this.emailService.SendPasswordResetAsync(user.Id, link, cancellationToken);
        }

        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // TODO error description
                return Result.Failure();
            }

            var result = await this.userManager.ResetPasswordAsync(user, token, newPassword);
            return result.ToResult();
        }

        public async Task<Result> DeleteAccountAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // TODO error description
                return Result.Failure($"User not exist'");
            }
            user.DeleteAccount = 1;
            var result = await this.userManager.UpdateAsync(user);
            return result.ToResult();
        }

        public async Task<Result<bool>> VerifyPasswordResetTokenAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // TODO error description
                return Result.Failure<bool>();
            }

            var isValid = await this.userManager.VerifyUserTokenAsync(
                user,
                options.Tokens.PasswordResetTokenProvider,
                UserManager<User>.ResetPasswordTokenPurpose,
                token
                );

            return Result.Success(isValid);
        }
    }
}
