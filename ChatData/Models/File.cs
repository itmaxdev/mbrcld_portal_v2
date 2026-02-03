using System;
using System.Collections.Generic;
using System.Text;

namespace ChatData.Models
{
    public class File : ChatBaseEntity
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}
