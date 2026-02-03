using AutoMapper;
using ChatData.Models;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class GetRoomMessagesQuery : IRequest<List<GetRoomMessageViewModel>>
    {
        public Guid RoomId { get; set; }
        public MessageTypes? MessageType { get; set; }

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetRoomMessagesQuery, List<GetRoomMessageViewModel>>
        {
            private readonly IMapper mapper;
            private readonly IChatRepository chatRepository;

            public QueryHandler(IMapper mapper, IChatRepository chatRepository)
            {
                this.mapper = mapper;
                this.chatRepository = chatRepository;
            }

            public async Task<List<GetRoomMessageViewModel>> Handle(GetRoomMessagesQuery request, CancellationToken cancellationToken)
            {
                var messages = await chatRepository.GetRoomMessages(request.RoomId, request.MessageType, cancellationToken);

                return mapper.Map<List<GetRoomMessageViewModel>>(messages);
            }
        }
        #endregion
    }
}
