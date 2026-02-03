using ChatData.Context;
using ChatData.Models;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        protected readonly ChatContext _context;

        public ChatRepository(ChatContext context)
        {
            _context = context;
        }

        public async Task<List<Guid>> GetRoomParticipantsIds(Guid roomId, CancellationToken cancellationToken = default)
        {
            var participants = _context.Set<Participants>().Where(p => p.RoomId == roomId);

            return await participants.Select(p => p.UserId).ToListAsync(cancellationToken);
        }

        public async Task<List<Room>> GetUserRoomsAsync(Guid userId, Guid? moduleId, CancellationToken cancellationToken = default)
        {
            var participants = _context.Set<Participants>().AsQueryable()
                                                           .AsNoTracking()
                                                           .Where(p => p.UserId == userId);

            if (moduleId.HasValue)
            {
                return await participants.Select(p => p.Room).Where(p => p.ModuleId == moduleId).ToListAsync(cancellationToken);
            }
            else
            {
                return await participants.Select(p => p.Room).ToListAsync(cancellationToken);
            }
        }

        public async Task<List<Room>> GetMeetingHubRoomsAsync(CancellationToken cancellationToken = default)
        {
            var rooms = await _context.Set<Room>().Include(r => r.Participants)
                                                  .Where(r => r.RoomType == RoomTypes.MeetingHub)
                                                  .ToListAsync(cancellationToken);
            return rooms;
        }

        public async Task<List<Room>> GetAlumniUserRoomsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var alumniModuleId = new Guid("12341234-1234-1234-1234-123412341234");
            var rooms = await _context.Set<Room>().Include(r => r.Participants)
                                            .Where(r => r.Participants.Any(p => p.UserId == userId) &&
                                                        r.ModuleId == alumniModuleId)
                                            .ToListAsync(cancellationToken);
            return rooms;
        }

        public async Task<List<Message>> GetRoomMessages(Guid roomId, MessageTypes? type, CancellationToken cancellationToken = default)
        {
            if (type != null)
            {
                return await _context.Set<Message>()
                    .Where(p => p.RoomId == roomId && p.MessageType == type)
                    .Include(p => p.File)
                    .OrderBy(p => p.Time)
                    .ToListAsync(cancellationToken);
            }

            return await _context.Set<Message>()
                .Where(p => p.RoomId == roomId)
                .Include(p => p.File)
                .OrderBy(p => p.Time)
                .ToListAsync(cancellationToken);
        }

        public async Task<Room> GetRoom(Guid roomId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Room>()
                .Include(r => r.Participants)
                .FirstOrDefaultAsync(x => x.Id == roomId, cancellationToken);
        }

        public async Task<Message> GetRoomLastMessage(Guid roomId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Message>()
                .Where(p => p.RoomId == roomId && p.MessageType == MessageTypes.Message)
                .OrderBy(p => p.Time)
                .LastOrDefaultAsync(cancellationToken);
        }

        public async Task<Message> SendMessage(Message message, CancellationToken cancellationToken = default)
        {
            var result = await _context.Set<Message>().AddAsync(message, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }

        public async Task UpdateMessages(List<Message> messages, CancellationToken cancellationToken = default)
        {
            _context.Set<Message>().UpdateRange(messages);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Message>> GetRoomFiles(Guid roomId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Message>()
                .Where(p => p.RoomId == roomId && p.MessageType == MessageTypes.File)
                .OrderBy(p => p.Time)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateLastReadedMessage(Guid readerId, Guid roomId, Guid messageId, DateTime date, CancellationToken cancellationToken = default)
        {
            var userRoomRel = await _context.Set<Participants>()
                .Where(p => p.RoomId == roomId && p.UserId == readerId)
                .FirstOrDefaultAsync();

            userRoomRel.LastMessageId = messageId;
            userRoomRel.LastMessageDate = date;

            _context.Set<Participants>().UpdateRange(userRoomRel);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetUserUnreadMessagesCount(Guid roomId, Guid userId, CancellationToken cancellationToken = default)
        {
            var userRoomRel = await _context.Set<Participants>()
              .Where(p => p.RoomId == roomId && p.UserId == userId)
              .FirstOrDefaultAsync(cancellationToken);

            var date = userRoomRel.LastMessageDate;

            return await _context.Set<Message>().Where(m => m.RoomId == roomId && m.Time > date).CountAsync(cancellationToken);
        }

        public async Task<Room> CreateRoom(Room room, CancellationToken cancellationToken)
        {
            var result = await _context.Set<Room>().AddAsync(room, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }

        public async Task AddFile(File file, CancellationToken cancellationToken)
        {
            await _context.Set<File>().AddAsync(file, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<File> GetFileByUrl(string url, CancellationToken cancellationToken)
        {
            return await _context.Set<File>().Where(p => p.Path == url).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Participants>> GetRoomParticipants(Guid roomId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Participants>().Where(p => p.RoomId == roomId).ToListAsync(cancellationToken);
        }

        public async Task<Room> IsRoomExist(List<Guid> userIds, Guid moduleId, CancellationToken cancellationToken = default)
        {
            var room = await _context.Set<Room>().Where(p =>
            p.ModuleId == moduleId &&
            p.Participants.Count == userIds.Count &&
            p.Participants.All(p => userIds.Contains(p.UserId))
            ).FirstOrDefaultAsync(cancellationToken);

            return room;
        }

        public async Task AddParticipant(User user, Guid roomId, CancellationToken cancellationToken)
        {
            await _context.Set<Participants>().AddAsync(new Participants
            {
                UserId = user.Id,
                FullName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                FullNameAr = user.FirstNameAr + " " + user.MiddleNameAr + " " + user.LastNameAr,
                UserAvater = user.ProfilePictureUniqueId,
                RoomId = roomId,
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveParticipant(Guid userId, Guid roomId, CancellationToken cancellationToken)
        {
            var participant = await _context.Set<Participants>().FirstAsync(p => p.UserId == userId && p.RoomId == roomId);
            if (participant != null)
            {
                _context.Set<Participants>().Remove(participant);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
