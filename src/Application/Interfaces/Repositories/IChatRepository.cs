using ChatData.Models;
using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task<List<Room>> GetAlumniUserRoomsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<Room>> GetUserRoomsAsync(Guid userId, Guid? moduleId, CancellationToken cancellationToken = default);
        Task<List<Message>> GetRoomMessages(Guid roomId, MessageTypes? type, CancellationToken cancellationToken = default);
        Task<Room> GetRoom(Guid roomId, CancellationToken cancellationToken = default);
        Task<List<Message>> GetRoomFiles(Guid roomId, CancellationToken cancellationToken = default);
        Task<int> GetUserUnreadMessagesCount(Guid roomId, Guid userId, CancellationToken cancellationToken = default);
        Task<List<Guid>> GetRoomParticipantsIds(Guid roomId, CancellationToken cancellationToken = default);
        Task<List<Participants>> GetRoomParticipants(Guid roomId, CancellationToken cancellationToken = default);
        Task<Message> SendMessage(Message message, CancellationToken cancellationToken = default);
        Task UpdateMessages(List<Message> messages, CancellationToken cancellationToken = default);
        Task UpdateLastReadedMessage(Guid readerId, Guid roomId, Guid messageId, DateTime date, CancellationToken cancellationToken = default);
        Task<Message> GetRoomLastMessage(Guid roomId, CancellationToken cancellationToken = default);
        Task<Room> CreateRoom(Room room, CancellationToken cancellationToken = default);
        Task<Room> IsRoomExist(List<Guid> userIds, Guid moduleId, CancellationToken cancellationToken = default);
        Task AddFile(File file, CancellationToken cancellationToken = default);
        Task<File> GetFileByUrl(string url, CancellationToken cancellationToken = default);
        Task AddParticipant(User user, Guid roomId, CancellationToken cancellationToken);
        Task RemoveParticipant(Guid userId, Guid roomId, CancellationToken cancellationToken);
        Task<List<Room>> GetMeetingHubRoomsAsync(CancellationToken cancellationToken);
    }
}
