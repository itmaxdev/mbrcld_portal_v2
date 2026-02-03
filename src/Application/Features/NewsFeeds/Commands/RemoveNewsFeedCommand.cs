using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.NewsFeeds.Commands
{
    public class RemoveNewsFeedCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveNewsFeedCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RemoveNewsFeedCommand, Result>
        {
            private readonly INewsFeedRepository newsFeedRepository;
            private readonly IAttachedPictureService attacheFileService;

            public CommandHandler(INewsFeedRepository newsFeedRepository, IAttachedPictureService attacheFileService)
            {
                this.newsFeedRepository = newsFeedRepository;
                this.attacheFileService = attacheFileService;
            }

            public async Task<Result> Handle(RemoveNewsFeedCommand request, CancellationToken cancellationToken)
            {
                await newsFeedRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                await attacheFileService.RemoveAttachedDocumentAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
