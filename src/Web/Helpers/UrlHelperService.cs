using Mbrcld.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mbrcld.Web.Helpers
{
    public sealed class UrlHelperService : IUrlHelperService
    {
        private readonly IUrlHelper urlHelper;

        public UrlHelperService(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public string GetAbsoluteUrlForAction(string actionName, object routeValues)
        {
            return this.urlHelper.Link(actionName, routeValues);
        }
    }
}
