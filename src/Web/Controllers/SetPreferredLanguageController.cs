using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mbrcld.Web.Controllers
{
    [Route("SetPreferredLanguage")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SetPreferredLanguageController : Controller
    {
        private static readonly Regex UrlPattern = new Regex(@"^\/[a-zA-Z]{2}\b(.+)$");

        private readonly IPreferredLanguageService preferredLanguageService;

        public SetPreferredLanguageController(IPreferredLanguageService preferredLanguageService)
        {
            this.preferredLanguageService = preferredLanguageService;
        }

        [HttpGet]
        public ActionResult Get(string lang, string referer)
        {
            if (!string.IsNullOrWhiteSpace(lang) && IsSupportedLanguage(lang))
            {
                this.preferredLanguageService.SetPreferredLanguage(lang);
                return LocalRedirectToLocale(lang.Split('-').First().ToLower(), referer);
            }

            return LocalRedirect("/");
        }

        private ActionResult LocalRedirectToLocale(string locale, string referer)
        {
            if (string.IsNullOrWhiteSpace(referer) &&
                Request.Headers.TryGetValue("Referer", out var httpReferer))
            {
                referer = httpReferer;
            }

            if (!string.IsNullOrWhiteSpace(referer))
            {
                var refererUri = new Uri(referer);
                var relativeUrl = refererUri.PathAndQuery + refererUri.Fragment;

                var matcher = UrlPattern.Match(relativeUrl);
                if (matcher.Success)
                {
                    return LocalRedirect($"/{locale}{matcher.Groups[1].Value}");
                }
            }

            return LocalRedirect($"/{locale}");
        }

        private bool IsSupportedLanguage(string lang)
        {
            return UILanguages.SupportedLanguages.Any(e => CompareStringsIgnoreCase(e, lang));
        }

        private static bool CompareStringsIgnoreCase(string lhs, string rhs)
        {
            return string.Equals(lhs, rhs, StringComparison.OrdinalIgnoreCase);
        }
    }
}
