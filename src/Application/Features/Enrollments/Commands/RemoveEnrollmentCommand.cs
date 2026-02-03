using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Enrollments.Commands
{
    public sealed class RemoveEnrollmentCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveEnrollmentCommand(Guid id)
        {
            this.Id = id;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<RemoveEnrollmentCommand, Result>
        {
            private readonly IEnrollmentRepository enrollmentRepository;

            public CommandHandler(IEnrollmentRepository enrollmentRepository)
            {
                this.enrollmentRepository = enrollmentRepository;
            }

            public async Task<Result> Handle(RemoveEnrollmentCommand request, CancellationToken cancellationToken)
            {
                await enrollmentRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}