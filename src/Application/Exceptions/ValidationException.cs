using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mbrcld.Application.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public readonly string[] Errors;

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validation errors have occurred") // TODO localize string
        {
            this.Errors = failures.Select(f => f.ErrorMessage).ToArray();
        }
    }
}
