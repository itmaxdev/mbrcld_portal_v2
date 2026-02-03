using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Mbrcld.Web.Swagger
{
    public sealed class SwaggerParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var supportedVersions = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<MapToApiVersionAttribute>()
                .SelectMany(attr => attr.Versions)
                .OrderByDescending(e => e)
                .ToList();
                
            if (!supportedVersions.Any())
            {
                return;
            }

            var defaultVersion = supportedVersions.First();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-version",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String",
                    Default = new OpenApiString(defaultVersion.ToString()),
                }
            });
        }
    }
}
