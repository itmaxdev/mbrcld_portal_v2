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
    public sealed class ListArtcilesQuery : IRequest<IList<ListArticlesViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListArtcilesQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListArtcilesQuery, IList<ListArticlesViewModel>>
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

            public async Task<IList<ListArticlesViewModel>> Handle(ListArtcilesQuery request, CancellationToken cancellationToken)
            {
                var articles = await articleRepository.ListArticlesAsync(cancellationToken);
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
                return mapper.Map<IEnumerable<ListArticlesViewModel>>(articles).ToList();
            }
        }
        #endregion
    }
}
