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
    public sealed class GetProgramByIdQuery : IRequest<GetProgramByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetProgramByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetProgramByIdQuery, GetProgramByIdViewModel>
        {
            private readonly IProgramRepository programRepository;
            private readonly IPanHistoryRepository panHistoryRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramRepository programRepository, IPanHistoryRepository panHistoryRepository, IMapper mapper)
            {
                this.programRepository = programRepository;
                this.panHistoryRepository = panHistoryRepository;
                this.mapper = mapper;
            }

            public async Task<GetProgramByIdViewModel> Handle(GetProgramByIdQuery request, CancellationToken cancellationToken)
            {
                var program = await programRepository.GetProgramByIdAsync(request.Id, cancellationToken);

                //if (article.HasValue)
                //{
                //    var articlelikes = await panHistoryRepository.ListPanHistoriesByArticlesAsync(article.Value.Id).ConfigureAwait(false);
                //    foreach (var likes in articlelikes)
                //    {
                //        if (likes.UserId == request.UserId)
                //        {
                //            article.Value.Liked = true;
                //            break;
                //        }
                //    }

                //    if (articlelikes.Count > 0)
                //    {
                //        article.Value.Likes = articlelikes.Count;
                //    }
                //}

                return mapper.Map<GetProgramByIdViewModel>(program);
            }
        }
        #endregion
    }
}
