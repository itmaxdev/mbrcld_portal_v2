using System;

namespace ChatData.Models
{
    public class Message
    {
        public Message()
        {
            Time = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public string Text { get; set; }
        public MessageTypes MessageType { get; set; }
        public DateTime Time { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public Guid FileId { get; set; }
        public virtual Room Room { get; set; }
        public virtual File File { get; set; }
    }
}
