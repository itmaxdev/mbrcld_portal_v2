using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Mbrcld.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Mbrcld.Web.Identity
{
    public sealed class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, role));
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
