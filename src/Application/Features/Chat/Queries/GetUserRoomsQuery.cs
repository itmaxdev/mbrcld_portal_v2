using AutoMapper;
using ChatData.Models;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class GetUserRoomsQuery : IRequest<List<GetUserRoomsViewModel>>
    {
        public Guid UserId { get; set; }
        public Guid? ModuleId { get; set; }

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<GetUserRoomsQuery, List<GetUserRoomsViewModel>>
        {
            private readonly IMapper mapper;
            private readonly IChatRepository chatRepository;

            public QueryHandler(IMapper mapper, IChatRepository chatRepository)
            {
                this.mapper = mapper;
                this.chatRepository = chatRepository;
            }

            public async Task<List<GetUserRoomsViewModel>> Handle(GetUserRoomsQuery request, CancellationToken cancellationToken)
            {
                var rooms = new List<Room>();

                if (request.ModuleId == new Guid("12341234-1234-1234-1234-123412341234"))
                {
                    rooms = await chatRepository.GetAlumniUserRoomsAsync(request.UserId, cancellationToken);
                }
                else
                {
                    rooms = await chatRepository.GetUserRoomsAsync(request.UserId, request.ModuleId, cancellationToken);
                }

                var viewModel = mapper.Map<List<GetUserRoomsViewModel>>(rooms);

                foreach (var room in viewModel)
                {
                    room.LastMessage = await chatRepository.GetRoomLastMessage(room.Id, cancellationToken);
                    room.UnreadMessagesCount = await chatRepository.GetUserUnreadMessagesCount(room.Id, request.UserId, cancellationToken);
                    var roomParticipants = await chatRepository.GetRoomParticipants(room.Id, cancellationToken);

                    room.Participants = mapper.Map<List<GetUserRoomsViewModel.ParticipantInfo>>(roomParticipants);

                    if (room.Participants.Count == 2)
                    {
                        var oppositeUser = room.Participants.Where(p => p.UserId != request.UserId).FirstOrDefault();

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
