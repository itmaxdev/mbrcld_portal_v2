using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Achievements.Commands
{
    public sealed class DeleteCommentCommand : IRequest<Result>
    {
        #region Command
        public Guid ContactId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? NewsFeedId { get; set; }
        public Guid? ArticleId { get; set; }
        public int Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string Comment { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<DeleteCommentCommand, Result>
        {
            private readonly IPanHistoryRepository PanHistoryRepository;

            public CommandHandler(IPanHistoryRepository PanHistoryRepository)
            {
                this.PanHistoryRepository = PanHistoryRepository;
            }

            public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {

                return await PanHistoryRepository.DeleteCommentAsync(request.ContactId,
                request.ArticleId, request.PostId, request.NewsFeedId, cancellationToken).ConfigureAwait(false);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddorRemoveLikeCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ContactId).NotNull().NotEmpty();
                //RuleFor(x => x.Action).NotNull().NotEmpty();
                //RuleFor(x => x.ActionDate).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
