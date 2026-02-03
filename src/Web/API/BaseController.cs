using Mbrcld.SharedKernel.Result;
using Mbrcld.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mbrcld.Web.API
{
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult FromResult(Result result)
        {
            return result.IsSuccess
                ? (ActionResult)NoContent()
                : UnprocessableEntity(new ApiProblemDetails(
                    title: "There was a problem processing your request.",
                    detail: "One or more errors have occurred. See errors property for more details.",
                    instance: Request.Path,
                    status: HttpStatusCode.UnprocessableEntity,
                    errors: result.Errors
                    ));
        }

        protected ActionResult<T> FromResult<T>(Result<T> result)
        {
            return result.IsSuccess
                ? (ActionResult)Ok(result.Value)
                : UnprocessableEntity(new ApiProblemDetails(
                    title: "There was a problem processing your request.",
                    detail: "One or more errors have occurred. See errors property for more details.",
                    instance: Request.Path,
                    status: HttpStatusCode.UnprocessableEntity,
                    errors: result.Errors
                    ));
        }
    }
}
