using Mbrcld.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Identity.Stores
{
    internal sealed class RoleStore : IRoleStore<Role>
    {
        private static readonly List<Role> RoleList = new List<Role>();

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            RoleList.Add(role);
            // TODO
            //role.Id = Guid.NewGuid();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            var roleInList = RoleList.FirstOrDefault(r => r.NormalizedName == role.NormalizedName);
            if (roleInList != null)
            {
                RoleList.Remove(roleInList);
            }
            return IdentityResult.Success;
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            if (role.Id == default) return null;
            return role.Id.ToString();
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return role.Name;
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.SetName(roleName);
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return role.NormalizedName;
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            role.SetNormalizedName(normalizedName);
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var roleGuid = Guid.Parse(roleId);
            return RoleList.FirstOrDefault(r => r.Id == roleGuid);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return RoleList.FirstOrDefault(r => r.NormalizedName == normalizedRoleName);
        }

        public void Dispose()
        {
        }
    }
}
