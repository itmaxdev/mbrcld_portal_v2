using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TrainingCourses.Commands
{
    public class RemoveContentCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveContentCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RemoveContentCommand, Result>
        {
            private readonly IContentRepository contentRepository;
            private readonly IAttachedPictureService attacheFileService;

            public CommandHandler(IContentRepository contentRepository , IAttachedPictureService attacheFileService)
            {
                this.contentRepository = contentRepository;
                this.attacheFileService = attacheFileService;
            }

            public async Task<Result> Handle(RemoveContentCommand request, CancellationToken cancellationToken)
            {
                await contentRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                await attacheFileService.RemoveAttachedDocumentAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
