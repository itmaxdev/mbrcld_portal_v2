using System;
using System.Security.Claims;

namespace Mbrcld.Web.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User Id is empty");
            }

            return Guid.Parse(userId);
        }
    }
}
