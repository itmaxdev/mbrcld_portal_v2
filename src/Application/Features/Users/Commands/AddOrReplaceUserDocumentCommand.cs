using Mbrcld.Application.Interfaces;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class AddOrReplaceUserDocumentCommand : IRequest<Result>
    {
        #region Command
        public Guid UserId { get; }
        public string Identifier { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public CancellationToken CancellationToken { get; }

        public AddOrReplaceUserDocumentCommand(
            Guid userId,
            string identitifer,
            byte[] content,
            string contentType,
            string fileName
            )
        {
            this.UserId = userId;
            this.Identifier = identitifer;
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrReplaceUserDocumentCommand, Result>
        {
            private readonly IUserDocumentService userDocumentService;

            public CommandHandler(IUserDocumentService userDocumentService)
            {
                this.userDocumentService = userDocumentService;
            }

            public async Task<Result> Handle(AddOrReplaceUserDocumentCommand request, CancellationToken cancellationToken)
            {
                var documentIdentifier = DocumentIdentifier.FromValue(request.Identifier);
                if (documentIdentifier.IsFailure)
                {
                    return Result.Failure("Invalid document identifier");
                }

                return await this.userDocumentService.InsertOrReplaceDocumentAsync(
                    request.UserId,
                    documentIdentifier.Value,
                    request.Content,
                    request.ContentType,
                    request.FileName,
                    cancellationToken
                    );
            }
        }
        #endregion
    }
}
