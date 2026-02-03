using AutoMapper;
using Mbrcld.Application.Features.Events.Queries;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Scholarships.Queries
{
    public sealed class ListScholarshipQuery : IRequest<IList<ListScholarshipViewModel>>
    {
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListScholarshipQuery, IList<ListScholarshipViewModel>>
        {
            private readonly IScholarshipRepository scholarshipRepository;
            private readonly IMapper mapper;

            public QueryHandler(IScholarshipRepository scholarshipRepository, IMapper mapper)
            {
                this.scholarshipRepository = scholarshipRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListScholarshipViewModel>> Handle(ListScholarshipQuery request, CancellationToken cancellationToken)
            {
                var scholarships = await scholarshipRepository.GetScholarshipAsync(cancellationToken);
                return mapper.Map<IEnumerable<ListScholarshipViewModel>>(scholarships).ToList();
            }
        }
        #endregion
    }
}