using Mbrcld.SharedKernel.Result;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Mbrcld.Infrastructure.Extensions
{
    internal static class IdentityResultExtensions
    {
        internal static Result ToResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Code).ToArray());
        }
    }
}
