using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Identity.Stores
{
    internal sealed class UserStore : IUserStore<User>,
        IUserPasswordStore<User>,
        IUserEmailStore<User>,
        IUserSecurityStampStore<User>,
        IUserRoleStore<User>
    {
        private readonly IUserRepository userRepository;

        public UserStore(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return user.Id == default ? null : user.Id.ToString();
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.Email?.Value;
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.SetEmail(userName);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.SetNormalizedEmail(normalizedName);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await this.userRepository.CreateAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await this.userRepository.UpdateAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await this.userRepository.DeleteAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
            return user.ValueOrDefault;
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.GetByEmailAsync(normalizedUserName, cancellationToken);
            return user.ValueOrDefault;
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.SetPasswordHash(passwordHash);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return !string.IsNullOrEmpty(user.PasswordHash);
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.SetEmail(email);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return user.Email?.Value;
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.SetEmailConfirmed(confirmed);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
            return user.ValueOrDefault;
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.SetNormalizedEmail(normalizedEmail);
        }

        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            return user.SecurityStamp;
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            var role = "";
            switch(user.Role)
            {
                case 1: role = "Registrant";
                    break;

                case 2: role = "Applicant";
                    break;

                case 3: role = "Alumni";
                    break;

                case 4: role = "Instructor";
                    break;

                case 5:
                    role = "Direct Manager";
                    break;

                case 6:
                    role = "Admin";
                    break;
            }
            return new[] { role };
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
