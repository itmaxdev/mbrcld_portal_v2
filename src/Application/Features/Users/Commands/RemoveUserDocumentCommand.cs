using Mbrcld.Application.Interfaces;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class RemoveUserDocumentCommand : IRequest<Result>
    {
        public Guid UserId { get; }
        public string Identifier { get; }

        public RemoveUserDocumentCommand(Guid userId, string documentIdentifier)
        {
            this.UserId = userId;
            this.Identifier = documentIdentifier;
        }

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<RemoveUserDocumentCommand, Result>
        {
            private readonly IUserDocumentService userDocumentService;

            public CommandHandler(IUserDocumentService userDocumentService)
            {
                this.userDocumentService = userDocumentService;
            }

            public async Task<Result> Handle(RemoveUserDocumentCommand request, CancellationToken cancellationToken)
            {
                var documentIdentifier = DocumentIdentifier.FromValue(request.Identifier);
                if (documentIdentifier.IsFailure)
                {
                    return Result.Failure("Invalid document identifier");
                }

                return await this.userDocumentService.RemoveDocumentAsync(request.UserId, documentIdentifier.Value, cancellationToken);
            }
        }
        #endregion
    }
}
