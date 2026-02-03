using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Linq;

namespace Mbrcld.Web.Helpers
{
    // Workaround for OData and Swagger
    // see https://github.com/OData/WebApi/issues/1177#issuecomment-358659774
    internal sealed class ODataSwaggerMvcFix
    {
        private readonly MvcOptions options;

        public ODataSwaggerMvcFix(MvcOptions options)
        {
            this.options = options;
        }

        public void Apply()
        {
            var odataInputFormatters = options.InputFormatters.OfType<ODataInputFormatter>()
                    .Where(e => e.SupportedMediaTypes.Count == 0);

            foreach (var inputFormatter in odataInputFormatters)
            {
                inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            }

            var odataOutputFormatters = options.OutputFormatters.OfType<ODataOutputFormatter>()
                .Where(e => e.SupportedMediaTypes.Count == 0);

            foreach (var outputFormatter in odataOutputFormatters)
            {
                outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            }
        }
    }
}
