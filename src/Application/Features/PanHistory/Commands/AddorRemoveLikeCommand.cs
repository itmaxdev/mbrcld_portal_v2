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
    public sealed class AddorRemoveLikeCommand : IRequest<Result>
    {
        #region Command
        public Guid ContactId { get; set; }
        public Guid? ArticleId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? NewsFeedId { get; set; }
        public int Action { get; set; }
        public bool Like { get; set; }
        public DateTime ActionDate { get; set; }
        public string? Comment { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddorRemoveLikeCommand, Result>
        {
            private readonly IPanHistoryRepository PanHistoryRepository;

            public CommandHandler(IPanHistoryRepository PanHistoryRepository)
            {
                this.PanHistoryRepository = PanHistoryRepository;
            }

            public async Task<Result> Handle(AddorRemoveLikeCommand request, CancellationToken cancellationToken)
            {
                if (request.Like == true)
                {
                    var panHistory = PanHistory.Create(
                    userid: request.ContactId,
                    articleid: request.ArticleId,
                    postid: request.PostId,
                    newsfeedid: request.NewsFeedId,
                    action: 1, //Like
                    actionDate:DateTime.UtcNow,
                    comment: request.Comment
                );

                    return await PanHistoryRepository.CreateAsync(panHistory).ConfigureAwait(false);
                    
                }
                else if (request.Like == false)
                {
                    return await PanHistoryRepository.DeleteAsync(request.ContactId, request.ArticleId, request.PostId, request.NewsFeedId).ConfigureAwait(false);
                }
                return Result.Failure("Like Cannot be Empty");
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
