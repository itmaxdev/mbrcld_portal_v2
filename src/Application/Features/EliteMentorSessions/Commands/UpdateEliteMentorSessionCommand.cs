using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.EliteMentorSessions.Commands
{
    public sealed class UpdateEliteMentorSessionCommand : IRequest<Result>
    {
        #region Command
        public Guid EliteMentorSessionId { get; set; }
        public DateTime Date { get; set; }

        public UpdateEliteMentorSessionCommand(
            Guid eliteMentorSessionId,
            DateTime date
            )
        {
            this.EliteMentorSessionId = eliteMentorSessionId;
            this.Date = date;
        }

        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<UpdateEliteMentorSessionCommand, Result>
        {
            private readonly IEliteMentorSessionRepository eliteMentorSessionRepository;

            public CommandHandler(IEliteMentorSessionRepository eliteMentorSessionRepository)
            {
                this.eliteMentorSessionRepository = eliteMentorSessionRepository;
            }

            public async Task<Result> Handle(UpdateEliteMentorSessionCommand request, CancellationToken cancellationToken)
            {
                var eliteMentorSession = await eliteMentorSessionRepository.GetEliteMentorSessionByIdAsync(request.EliteMentorSessionId, cancellationToken);
                if (eliteMentorSession.HasNoValue)
                {
                    return Result.Failure($"Invalid Elite Mentor Session with ID { request.EliteMentorSessionId}");
                }

                var eliteMentorSessionValue = eliteMentorSession.Value;
                eliteMentorSessionValue.Date = request.Date;

                await eliteMentorSessionRepository.UpdateAsync(eliteMentorSessionValue).ConfigureAwait(false);

                return Result.Success(eliteMentorSession.Value.Id);
            }
        }
        #endregion
    }
}
