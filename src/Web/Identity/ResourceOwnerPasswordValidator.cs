using System;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Mbrcld.Domain.Entities;
using Mbrcld.Web.UAE;
using Microsoft.AspNetCore.Identity;
using static IdentityModel.OidcConstants;

namespace Mbrcld.Web.Identity
{
    /// <summary>
    /// IResourceOwnerPasswordValidator that integrates with ASP.NET Identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <seealso cref="IdentityServer4.Validation.IResourceOwnerPasswordValidator" />
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IEventService _events;
        private readonly UserManager<User> _userManager;
        private readonly IUAEService _uaeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        public ResourceOwnerPasswordValidator(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEventService events,
            IUAEService uaeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _uaeService = uaeService;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
            if (result.Succeeded)
            {
                var sub = await _userManager.GetUserIdAsync(user);

                await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                return;
            }
            else
            {
                // If password is not valid, check if its token for UAE auth
                var token = context.Password;
                var profile = await _uaeService.GetProfile(token);

                if (profile != null)
                {
                    var alreadyAddedUser = await _userManager.FindByEmailAsync(profile.Email);
                    if (alreadyAddedUser != null)
                    {
                        var sub = await _userManager.GetUserIdAsync(user);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));
                        context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                        
                        return;
                    }
                }
            }

            if (result.IsLockedOut)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
            }
            else if (result.IsNotAllowed)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}
