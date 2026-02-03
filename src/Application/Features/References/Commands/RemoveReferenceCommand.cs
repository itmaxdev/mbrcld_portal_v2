using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.References.Commands
{
    public sealed class RemoveReferenceCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveReferenceCommand(Guid id)
        {
            this.Id = id;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<RemoveReferenceCommand, Result>
        {
            private readonly IReferenceRepository referenceRepository;

            public CommandHandler(IReferenceRepository referenceRepository)
            {
                this.referenceRepository = referenceRepository;
            }

            public async Task<Result> Handle(RemoveReferenceCommand request, CancellationToken cancellationToken)
            {
                await referenceRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
