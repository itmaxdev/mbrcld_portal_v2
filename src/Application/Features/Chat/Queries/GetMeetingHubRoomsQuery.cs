using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class GetMeetingHubRoomsQuery : IRequest<List<GetUserRoomsViewModel>>
    {
        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetMeetingHubRoomsQuery, List<GetUserRoomsViewModel>>
        {
            private readonly IMapper mapper;
            private readonly IChatRepository chatRepository;

            public QueryHandler(IMapper mapper, IChatRepository chatRepository)
            {
                this.mapper = mapper;
                this.chatRepository = chatRepository;
            }

            public async Task<List<GetUserRoomsViewModel>> Handle(GetMeetingHubRoomsQuery request, CancellationToken cancellationToken)
            {
                var rooms = await chatRepository.GetMeetingHubRoomsAsync(cancellationToken);
                var viewModel = mapper.Map<List<GetUserRoomsViewModel>>(rooms);
                return viewModel;
            }
        }
        #endregion
    }
}
