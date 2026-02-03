using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Mbrcld.Web.Middleware
{
    public class SpaMultiLanguageMiddleware
    {
        private readonly IPreferredLanguageService preferredLanguageService;
        private readonly ILogger<SpaMultiLanguageMiddleware> logger;
        private readonly RequestDelegate next;

        public SpaMultiLanguageMiddleware(
            RequestDelegate next,
            IPreferredLanguageService preferredLanguageService,
            ILogger<SpaMultiLanguageMiddleware> logger)
        {
            this.next = next;
            this.preferredLanguageService = preferredLanguageService;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestedPath = context.Request.Path;

            if (context.Request.Path == "/")
            {
                var preferredLangLcid = this.preferredLanguageService.GetPreferredLanguageLCID();

                this.logger.LogInformation($"Request Path: {requestedPath}");
                this.logger.LogInformation($"Preferred Language: {preferredLangLcid}");
                this.logger.LogInformation($"Redirecting");

                if (preferredLangLcid == UILanguages.ArabicLCID)
                {
                    context.Request.Path = "/ar/";
                }
                else
                {
                    context.Request.Path = "/en/";
                }
            }

            this.logger.LogInformation($"Updated Request Path: {context.Request.Path}");

            await this.next(context);
        }
    }
}
