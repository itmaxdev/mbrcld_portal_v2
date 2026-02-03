using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Commands
{
    public sealed class AddEventAnswersCommand : IRequest<Result<Guid>>
    {
        #region Command
        public string Answer { get; set; }
        public Guid EventAnswerId { get; set; }
        public Guid EventId { get; set; }
        public Guid EventQuestionId { get; set; }
        public Guid EventRegistrantId { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddEventAnswersCommand, Result<Guid>>
        {
            private readonly IEventAnswersRepository eventanswers;

            public CommandHandler(IEventAnswersRepository eventanswers)
            {
                this.eventanswers = eventanswers;
            }

            public async Task<Result<Guid>> Handle(AddEventAnswersCommand request, CancellationToken cancellationToken)
            {
                var eventanswersRecord = EventAnswer.Create(request.Answer, request.EventId, request.EventQuestionId, request.EventRegistrantId);
                await eventanswers.CreateAsync(eventanswersRecord, cancellationToken).ConfigureAwait(false);
                return Result.Success(eventanswersRecord.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddEventAnswersCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Answer).NotNull().NotEmpty();
                RuleFor(x => x.EventId).NotNull().NotEmpty();
                RuleFor(x => x.EventQuestionId).NotNull().NotEmpty();
                RuleFor(x => x.EventRegistrantId).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}

