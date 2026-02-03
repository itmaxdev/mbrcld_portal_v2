using System;

namespace Mbrcld.Application.Exceptions
{
    public sealed class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {
        }
    }
}
