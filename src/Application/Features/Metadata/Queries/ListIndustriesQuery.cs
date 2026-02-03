using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListIndustriesQuery : IRequest<IList<ListIndustriesViewModel>>
    {
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListIndustriesQuery, IList<ListIndustriesViewModel>>
        {
            private readonly IMetadataRepository metadataRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMetadataRepository metadataRepository, IMapper mapper)
            {
                this.metadataRepository = metadataRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListIndustriesViewModel>> Handle(ListIndustriesQuery request, CancellationToken cancellationToken)
            {
                var industries = await metadataRepository.GetIndustriesAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListIndustriesViewModel>>(industries).ToList();
            }
        }
        #endregion
    }
}
