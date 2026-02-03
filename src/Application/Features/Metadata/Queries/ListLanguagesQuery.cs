using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListLanguagesQuery : IRequest<IList<ListLanguagesViewModel>>
    {
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListLanguagesQuery, IList<ListLanguagesViewModel>>
        {
            private readonly IMetadataRepository metadataRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMetadataRepository metadataRepository, IMapper mapper)
            {
                this.metadataRepository = metadataRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListLanguagesViewModel>> Handle(ListLanguagesQuery request, CancellationToken cancellationToken)
            {
                var industries = await metadataRepository.GetLanguagesAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListLanguagesViewModel>>(industries).ToList();
            }
        }
        #endregion
    }
}
