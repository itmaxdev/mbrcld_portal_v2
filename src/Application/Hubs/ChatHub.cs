using AutoMapper;
using ChatData.Models;
using Mbrcld.Application.Hubs;
using Mbrcld.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Mbrcld.Application
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly IChatRepository chatRepository;
        private readonly IMapper mapper;

        public ChatHub(IChatRepository chatRepository, IMapper mapper)
        {
            this.chatRepository = chatRepository;
            this.mapper = mapper;
        }

        public async Task BroadcastAsync(ChatMessage message)
        {
            await Clients.All.MessageReceivedFromHub(message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.NewUserConnected("a new user connected");
        }

        public async Task SendMessageToGroup(ChatMessage message, string groupName)
        {
            message.Id = Guid.NewGuid();

            var dbMessage = mapper.Map<Message>(message);

            await Clients.Group(groupName).HandleMessage(message);

            await chatRepository.SendMessage(dbMessage);
        }

        public async Task MarkMessageAsReaded(ChatMessage message, Guid readerId)
        {
            await chatRepository.UpdateLastReadedMessage(readerId, message.RoomId, message.Id, message.DateTime);
        }

        public async Task CreateGroup(string connectionId, string groupName)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task UploadFile(string fileUrl, string groupName, string userId)
        {
            var file = await chatRepository.GetFileByUrl(fileUrl);

            var uploadedFile = new UploadedFile { FileName = file.FileName, Path = file.Path, GroupName = groupName, UserId = userId };

            await Clients.Group(groupName).HandleUploadedFile(uploadedFile);
        }
    }

    public interface IChatHub
    {
        Task MessageReceivedFromHub(ChatMessage message);

        Task NewUserConnected(string message);

        Task HandleMessage(ChatMessage message);

        Task HandleUploadedFile(UploadedFile file);
    }
}
