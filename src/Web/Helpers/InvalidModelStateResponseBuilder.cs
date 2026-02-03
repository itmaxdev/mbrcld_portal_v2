using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Mbrcld.Web.Helpers
{
    internal sealed class InvalidModelStateResponseBuilder
    {
        private readonly ActionContext context;

        public InvalidModelStateResponseBuilder(ActionContext context)
        {
            this.context = context;
        }

        public IActionResult Build()
        {
            var problemDetailsFactory = context.HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();

            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext, context.ModelState);

            problemDetails.Instance = context.HttpContext.Request.Path;

            var actionExecutingContext = context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

            var hasModelStateErrors = context.ModelState.ErrorCount > 0;
            var hasMissingArgs = actionExecutingContext?.ActionArguments.Count != context.ActionDescriptor.Parameters.Count;

            if (hasModelStateErrors && (context is ControllerContext || !hasMissingArgs))
            {
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            }

            problemDetails.Status = StatusCodes.Status400BadRequest;
            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }
    }
}
