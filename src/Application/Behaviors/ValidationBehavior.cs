using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Behaviors
{
    internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (this.validators.Any())
            {
                var context = new FluentValidation.ValidationContext<TRequest>(request);
                var results = await Task.WhenAll(this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var errors = results.SelectMany(r => r.Errors).Where(e => e != null).ToList();
                if (errors.Any())
                {
                    throw new Exceptions.ValidationException(errors);
                }
            }

            return await next();
        }
    }
}
