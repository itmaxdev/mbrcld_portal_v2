using AutoMapper;
using Mbrcld.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries
{
    public class ListUserDocumentsQuery : IRequest<IList<ListUserDocumentsViewModel>>
    {
        public Guid UserId { get; }

        public ListUserDocumentsQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public sealed class QueryHandler : IRequestHandler<ListUserDocumentsQuery, IList<ListUserDocumentsViewModel>>
        {
            private readonly IUserDocumentService userDocumentService;
            private readonly IMapper mapper;

            public QueryHandler(IUserDocumentService userDocumentService, IMapper mapper)
            {
                this.userDocumentService = userDocumentService;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserDocumentsViewModel>> Handle(ListUserDocumentsQuery request, CancellationToken cancellationToken)
            {
                var documents = await this.userDocumentService.ListDocumentsAsync(request.UserId, cancellationToken);
                return this.mapper.Map<IList<ListUserDocumentsViewModel>>(documents);
            }
        }
    }
}
