using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListUserArticlesQuery : IRequest<IList<ListUserArticlesViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListUserArticlesQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListUserArticlesQuery, IList<ListUserArticlesViewModel>>
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

            public async Task<IList<ListUserArticlesViewModel>> Handle(ListUserArticlesQuery request, CancellationToken cancellationToken)
            {
                var articles = await articleRepository.ListUserArticlesAsync(request.UserId, cancellationToken);
                foreach (var article in articles)
                {
                    //var userlike = await panHistoryRepository.GetPanHistoryByUserIdAndArticleIdAsync(request.UserId, article.Id).ConfigureAwait(false);
                    var articlelikes = await panHistoryRepository.ListPanHistoriesByArticlesAsync(article.Id).ConfigureAwait(false);
                    foreach(var likes in articlelikes)
                    {
                        if (likes.UserId == request.UserId)
                        {
                            article.Liked = true;
                            break;
                        }
                    }

                    if (articlelikes.Count > 0)
                    {
                        article.Likes = articlelikes.Count;
                    }
                }
                return mapper.Map<IEnumerable<ListUserArticlesViewModel>>(articles).ToList();
            }
        }
        #endregion
    }
}
