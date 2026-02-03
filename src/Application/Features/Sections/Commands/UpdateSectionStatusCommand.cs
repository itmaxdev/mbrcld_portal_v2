using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class UpdateSectionStatusCommand : IRequest<Result>
    {
        #region Command
        public Guid SectionId { get; }
        public Guid ContactId { get; set; }
        public int Status { get; set; }
        public CancellationToken CancellationToken { get; }

        public UpdateSectionStatusCommand(
            Guid sectionId,
            Guid userId,
            int status
            )
        {
            this.SectionId = sectionId;
            this.ContactId = userId;
            this.Status = status;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<UpdateSectionStatusCommand, Result>
        {
            private readonly ISectionRepository sectionRepository;

            public CommandHandler(ISectionRepository sectionRepository)
            {
                this.sectionRepository = sectionRepository;
            }

            public async Task<Result> Handle(UpdateSectionStatusCommand request, CancellationToken cancellationToken)
            {
                await sectionRepository.UpdateSectionStatusAsync(request.SectionId, request.ContactId, request.Status, cancellationToken);
                return Result.Success(request.SectionId);
            }
        }
    }
}
#endregion