using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class PostRepository : IPostRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public PostRepository(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Maybe<Post>> GetPostByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataPost = await webApiClient.For<ODataPost>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Post>(odataPost);
        }


        public async Task<IList<Post>> ListPostsAsync(CancellationToken cancellationToken = default)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            var odataPosts = await webApiClient.For<ODataPost>()
                .Filter(c => c.PostStatus == 2) //Published
                .Filter(c => c.ExpiryDate > yesterday || c.ExpiryDate == null)
                .Filter(c => c.PostDate <= DateTime.Today || c.PostDate == null)
                .OrderByDescending(x => x.PostDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Post>>(odataPosts);
        }

        public async Task<Post> PinnedPostsAsync(CancellationToken cancellationToken = default)
        {
            try
            {            
            DateTime yesterday = DateTime.Today.AddDays(-1);
            var odataPosts = await webApiClient.For<ODataPost>()
                .Filter(c => c.PostStatus == 2) //Published
                .Filter(c => c.ExpiryDate > yesterday || c.ExpiryDate == null)
                .Filter(c => c.PostDate <= DateTime.Today || c.PostDate == null)
                .Filter(c=>c.Pinned==true)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Post>(odataPosts.FirstOrDefault());
            }
            catch (Exception ex)
            {

                return null;
                
            }
        }

        public async Task<IList<Post>> SearchPostsAsync(string search, CancellationToken cancellationToken = default)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            var odataPosts = await webApiClient.For<ODataPost>()
                .Filter(c => c.Name.Contains(search) || c.Desription.Contains(search))
                .Filter(c => c.PostStatus == 2) //Published
                .Filter(c => c.ExpiryDate > yesterday || c.ExpiryDate == null)
                .Filter(c => c.PostDate <= DateTime.Today || c.PostDate == null)
                .OrderByDescending(x => x.PostDate)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Post>>(odataPosts);
        }
    }
}