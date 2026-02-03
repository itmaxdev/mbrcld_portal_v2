using Mbrcld.Web.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Mbrcld.Web.Middleware
{
    public class ProblemDetailsMiddleware
    {
        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        private static readonly List<string> AllowedHeaders = new List<string>()
        {
            HeaderNames.AccessControlAllowCredentials,
            HeaderNames.AccessControlAllowHeaders,
            HeaderNames.AccessControlAllowMethods,
            HeaderNames.AccessControlAllowOrigin,
            HeaderNames.AccessControlExposeHeaders,
            HeaderNames.AccessControlMaxAge,

            HeaderNames.StrictTransportSecurity,

            HeaderNames.WWWAuthenticate,

            "api-supported-versions",

            "X-Rate-Limit-Limit",
            "X-Rate-Limit-Remaining",
            "X-Rate-Limit-Reset",
        };

        private readonly RequestDelegate next;
        private readonly IActionResultExecutor<ObjectResult> executor;
        private readonly IWebHostEnvironment env;

        public ProblemDetailsMiddleware(
            RequestDelegate next,
            IActionResultExecutor<ObjectResult> executor,
            IWebHostEnvironment env)
        {
            this.next = next;
            this.executor = executor;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            ExceptionDispatchInfo edi = null;

            try
            {
                await this.next(context);
            }
            catch (Exception e)
            {
                edi = ExceptionDispatchInfo.Capture(e);
            }

            if (edi != null)
            {
                await HandleException(context, edi);
            }
            else if (IsProblem(context))
            {
                await this.HandleProblem(context);
            }
        }

        private async Task HandleProblem(HttpContext context)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            int statusCode = context.Response.StatusCode;
            this.ClearResponse(context, statusCode);

            var details = new ApiProblemDetails(
                ReasonPhrases.GetReasonPhrase(statusCode),
                default,
                context.Request.Path,
                statusCode,
                default
                );

            await this.WriteProblemDetails(context, details);
        }

        private async Task HandleException(HttpContext context, ExceptionDispatchInfo edi)
        {
            if (context.Response.HasStarted)
            {
                edi.Throw();
            }

            this.ClearResponse(context, 500);

            var exceptions = new List<Exception>();
            if (env.IsDevelopment())
            {
                exceptions.Add(edi.SourceException);
            }

            var details = new ApiProblemDetails(
                ReasonPhrases.GetReasonPhrase(500),
                edi.SourceException.Message,
                context.Request.Path,
                HttpStatusCode.InternalServerError,
                exceptions.ToArray()
                );

            await this.WriteProblemDetails(context, details);
        }

        private async Task WriteProblemDetails(HttpContext context, ApiProblemDetails details)
        {
            var actionContext = new ActionContext(context, context.GetRouteData(), EmptyActionDescriptor);

            var objectResult = new ObjectResult(details)
            {
                StatusCode = details.Status,
                ContentTypes = { "application/problem+json" },
                DeclaredType = details.GetType(),
            };

            await this.executor.ExecuteAsync(actionContext, objectResult);
        }

        private void ClearResponse(HttpContext context, int statusCode)
        {
            var headers = new HeaderDictionary()
            {
                { HeaderNames.CacheControl, "no-cache, no-store, must-revalidate" },
                { HeaderNames.Pragma, "no-cache" },
                { HeaderNames.Expires, "0" },
            };

            foreach (var header in context.Response.Headers)
            {
                if (AllowedHeaders.Contains(header.Key))
                {
                    headers.Add(header);
                }
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;

            foreach (var header in headers)
            {
                context.Response.Headers.Add(header);
            }
        }

        private bool IsProblem(HttpContext context)
        {
            return context.Response.StatusCode >= 400;
        }
    }
}
