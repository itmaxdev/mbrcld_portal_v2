using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Mbrcld.Domain.Entities.EventRegistrant;

namespace Mbrcld.Application.Features.Enrollments.Commands
{
    public sealed class AddEventRegistrantCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddEventRegistrantCommand, Result<Guid>>
        {
            private readonly IEventRegistrantRepository eventregistrant;

            public CommandHandler(IEventRegistrantRepository eventregistrant)
            {
                this.eventregistrant = eventregistrant;
            }

            public async Task<Result<Guid>> Handle(AddEventRegistrantCommand request, CancellationToken cancellationToken)
            {
                var eventregistranttRecord = EventRegistrant.Create(request.Id, request.EventId);
                await eventregistrant.CreateAsync(eventregistranttRecord, cancellationToken).ConfigureAwait(false);
                return Result.Success(eventregistranttRecord.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddEventRegistrantCommand>
        {
            public CommandValidator()
            {                
                RuleFor(x => x.EventId).NotNull().NotEmpty();
                RuleFor(x => x.Id).NotNull().NotEmpty();  
            }
        }
        #endregion
    }
}
