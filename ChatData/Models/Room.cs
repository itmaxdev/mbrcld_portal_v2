using System;
using System.Collections.Generic;

namespace ChatData.Models
{
    public class Room : ChatBaseEntity
    {
        public Room()
        {
            Participants = new HashSet<Participants>();
            Messages = new HashSet<Message>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Guid ModuleId { get; set; }
        public RoomTypes RoomType { get; set; } = RoomTypes.Module;
        public virtual ICollection<Participants> Participants { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
