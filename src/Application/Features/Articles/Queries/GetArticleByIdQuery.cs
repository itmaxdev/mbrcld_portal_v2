using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetArticleByIdQuery : IRequest<GetArticleByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetArticleByIdQuery(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetArticleByIdQuery, GetArticleByIdViewModel>
        {
            private readonly IArticleRepository articleRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IArticleRepository articleRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.articleRepository = articleRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<GetArticleByIdViewModel> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
            {
                var article = await articleRepository.GetArticleByIdAsync(request.Id, cancellationToken);
                if (article.HasValue)
                {
                    var articlelikes = await panHistoryRepository.ListPanHistoriesByArticlesAsync(article.Value.Id).ConfigureAwait(false);
                    foreach (var likes in articlelikes)
                    {
                        if (likes.UserId == request.UserId)
                        {
                            article.Value.Liked = true;
                            break;
                        }
                    }

                    if (articlelikes.Count > 0)
                    {
                        article.Value.Likes = articlelikes.Count;
                    }
                }
                return mapper.Map<GetArticleByIdViewModel>(article.ValueOrDefault);
            }
        }
        #endregion
    }
}
