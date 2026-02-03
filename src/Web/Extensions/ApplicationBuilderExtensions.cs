using Mbrcld.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Mbrcld.Web.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static void UseProblemDetails(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ProblemDetailsMiddleware>();
        }

        internal static void UseSpaMultiLanguage(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SpaMultiLanguageMiddleware>();
        }
    }
}
