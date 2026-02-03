using System;
using System.Collections.Generic;
using System.Text;

namespace ChatData.Models
{
    public class Participants : ChatBaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public Guid? LastMessageId { get; set; }
        public DateTime? LastMessageDate { get; set; }
        public string FullName { get; set; }
        public string FullNameAr { get; set; }
        public string UserAvater { get; set; }
        public virtual Room Room { get; set; }
    }
}
