using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListPostsQuery : IRequest<IList<ListPostsViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListPostsQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion
        #region Query Handler

        private sealed class QueryHandler : IRequestHandler<ListPostsQuery, IList<ListPostsViewModel>>
        {
            private readonly IArticleRepository articleRepository;
            private readonly IPostRepository postRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IArticleRepository articleRepository, IPanHistoryRepository panHistoryRepository, IPostRepository postRepository, IMapper mapper)
            {
                this.articleRepository = articleRepository;
                this.postRepository = postRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListPostsViewModel>> Handle(ListPostsQuery request, CancellationToken cancellationToken)
            {
                //var articles = await articleRepository.ListArticlesAsync(cancellationToken);
                //foreach (var article in articles)
                //{
                //    var articlelikes = await panHistoryRepository.ListPanHistoriesByArticlesAsync(article.Id).ConfigureAwait(false);
                //    foreach(var likes in articlelikes)
                //    {
                //        if (likes.UserId == request.UserId)
                //        {
                //            article.Liked = true;
                //            break;
                //        }
                //    }

                //    if (articlelikes.Count > 0)
                //    {
                //        article.Likes = articlelikes.Count;
                //    }
                //}

                //var mapped = this.mapper.Map<IList<Post>>(articles);
                var posts = await postRepository.ListPostsAsync(cancellationToken);
                foreach (var post in posts)
                {
                    post.Type = 2;//Post
                    post.WrittenBy = null;
                    if (post.Likes == null)
                        post.Likes = 0;
                    // var postlikes = await panHistoryRepository.ListPanHistoriesByPostsAsync(post.Id).ConfigureAwait(false);
                    ////foreach (var likes in postlikes)
                    ////{
                    ////    if (likes.UserId == request.UserId)
                    ////    {
                    ////        post.Liked = true;
                    ////        break;
                    ////    }
                    ////}
                    //post.Liked= postlikes.Where(t => t.UserId == request.UserId).Any();
                    //if (postlikes.Count > 0)
                    //{
                    //    post.Likes = postlikes.Count;
                    //}
                    var likedPosts =await panHistoryRepository.CheckIfPanHistoriesosLikedAsync(post.Id,request.UserId).ConfigureAwait(false);
                    post.Liked= likedPosts.Count > 0;
                }

                //foreach(var record in mapped)
                //{
                //    record.Type = 1;//Article
                //    record.ExpiryDate = null;
                //    posts.Add(record);
                //}
                var result = posts.OrderByDescending(x => x.PostDate);
                return mapper.Map<IEnumerable<ListPostsViewModel>>(result).ToList();
            }
        }
        #endregion
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Article, Post>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.PostDate, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.WrittenBy, x => x.MapFrom(src => src.WrittenBy))
                    .ForMember(dst => dst.WrittenByName, x => x.MapFrom(src => src.WrittenByName));
            }
        }
    }
}
