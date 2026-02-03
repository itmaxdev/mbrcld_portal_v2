using Mbrcld.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Linq;

namespace Mbrcld.Web.Services
{
    public sealed class PreferredLanguageService : IPreferredLanguageService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public PreferredLanguageService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int GetPreferredLanguageLCID()
        {
            var lcid = GetPreferredLanguageFromRequestHeader();
            if (lcid == null)
            {
                lcid = GetPreferredLanguageFromCookie();
            }

            return lcid ?? 1033;
        }

        public void SetPreferredLanguage(string locale)
        {
            this.httpContextAccessor.HttpContext.Response.Cookies.Append("User.Language", locale);
        }

        private int? GetPreferredLanguageFromCookie()
        {
            if (this.httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("User.Language", out var value))
            {
                return GetLanguageLCID(value);
            }

            return null;
        }

        private int? GetPreferredLanguageFromRequestHeader()
        {
            if (this.httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Accept-Language", out var value))
            {
                return GetLanguageLCID(value.ToString().Split(' ', ',').First());
            }

            return null;
        }

        private int? GetLanguageLCID(string name)
        {
            try
            {
                return CultureInfo.GetCultureInfo(name).LCID;
            }
            catch (CultureNotFoundException)
            {
                return null;
            }
        }
    }
}
