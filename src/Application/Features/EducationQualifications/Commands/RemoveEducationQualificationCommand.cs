using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.EducationQualifications.Commands
{
    public sealed class RemoveEducationQualificationCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveEducationQualificationCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<RemoveEducationQualificationCommand, Result>
        {
            private readonly IEducationQualificationRepository educationQualificationRepository;

            public CommandHandler(IEducationQualificationRepository educationQualificationRepository)
            {
                this.educationQualificationRepository = educationQualificationRepository;
            }

            public async Task<Result> Handle(RemoveEducationQualificationCommand request, CancellationToken cancellationToken)
            {
                await educationQualificationRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
