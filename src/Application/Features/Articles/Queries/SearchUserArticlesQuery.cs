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
    public sealed class SearchUserArticlesQuery : IRequest<IList<SearchUserArticlesViewModel>>
    {
        #region Query
        public string Search { get; }
        public Guid UserId { get; }

        public SearchUserArticlesQuery(string search, Guid userid)
        {
            Search = search;
            UserId = userid;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<SearchUserArticlesQuery, IList<SearchUserArticlesViewModel>>
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

            public async Task<IList<SearchUserArticlesViewModel>> Handle(SearchUserArticlesQuery request, CancellationToken cancellationToken)
            {
                var articles = await articleRepository.SearchUserArticlesAsync(request.UserId, request.Search, cancellationToken);
                foreach (var article in articles)
                {
                    //var userlike = await panHistoryRepository.GetPanHistoryByUserIdAndArticleIdAsync(request.UserId, article.Id).ConfigureAwait(false);
                    var articlelikes = await panHistoryRepository.ListPanHistoriesByArticlesAsync(article.Id).ConfigureAwait(false);
                    foreach (var likes in articlelikes)
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
                return mapper.Map<IEnumerable<SearchUserArticlesViewModel>>(articles).ToList();
            }
        }
        #endregion
    }
}
