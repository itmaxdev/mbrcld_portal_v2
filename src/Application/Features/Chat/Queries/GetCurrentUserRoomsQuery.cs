using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class GetCurrentUserRoomsQuery : IRequest<List<GetUserRoomsViewModel>>
    {
        public Guid CurrentUserId { get; set; }
        public Guid ExcludeUserId { get; set; }

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetCurrentUserRoomsQuery, List<GetUserRoomsViewModel>>
        {
            private readonly IMapper mapper;
            private readonly IChatRepository chatRepository;

            public QueryHandler(IMapper mapper, IChatRepository chatRepository)
            {
                this.mapper = mapper;
                this.chatRepository = chatRepository;
            }

            public async Task<List<GetUserRoomsViewModel>> Handle(GetCurrentUserRoomsQuery request, CancellationToken cancellationToken)
            {
                var rooms = await chatRepository.GetUserRoomsAsync(request.CurrentUserId, request.ExcludeUserId, cancellationToken);
                var viewModel = mapper.Map<List<GetUserRoomsViewModel>>(rooms);

                foreach (var room in viewModel)
                {
                    room.LastMessage = await chatRepository.GetRoomLastMessage(room.Id, cancellationToken);
                    room.UnreadMessagesCount = await chatRepository.GetUserUnreadMessagesCount(room.Id, request.CurrentUserId, cancellationToken);
                    var roomParticipants = await chatRepository.GetRoomParticipants(room.Id, cancellationToken);

                    room.Participants = mapper.Map<List<GetUserRoomsViewModel.ParticipantInfo>>(roomParticipants);

                    if (room.Participants.Count == 2)
                    {
                        var oppositeUser = room.Participants.Where(p => p.UserId != request.CurrentUserId).FirstOrDefault();

                        room.Name = oppositeUser.FullName;
                        room.ChatAvatar = oppositeUser.UserAvater;
                    }
                }

                return viewModel;
            }
        }
        #endregion
    }
}
