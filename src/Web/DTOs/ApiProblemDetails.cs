using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Mbrcld.Web.DTOs
{
    public sealed class ApiProblemDetails : ProblemDetails
    {
        private static readonly string[] EmptyErrorsArray = Array.Empty<string>();

        public object[] Errors { get; }

        public ApiProblemDetails(string title, string detail, string instance, int status, object[] errors)
        {
            this.Title = title;
            this.Detail = detail;
            this.Instance = instance;
            this.Status = status;
            this.Type = $"https://httpstatuses.com/{(int)status}";
            this.Errors = errors ?? EmptyErrorsArray;
        }

        public ApiProblemDetails(string title, string detail, string instance, HttpStatusCode status, object[] errors)
            : this(title, detail, instance, (int)status, errors)
        {
        }
    }
}
