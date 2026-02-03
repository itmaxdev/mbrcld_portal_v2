using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListSectorsQuery : IRequest<IList<ListSectorsViewModel>>
    {
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListSectorsQuery, IList<ListSectorsViewModel>>
        {
            private readonly IMetadataRepository metadataRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMetadataRepository metadataRepository, IMapper mapper)
            {
                this.metadataRepository = metadataRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListSectorsViewModel>> Handle(ListSectorsQuery request, CancellationToken cancellationToken)
            {
                var sectors = await metadataRepository.GetSectorsAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListSectorsViewModel>>(sectors).ToList();
            }
        }
        #endregion
    }
}
