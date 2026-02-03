using System;
using System.ComponentModel.DataAnnotations;

namespace ChatData.Models
{
    public class ChatBaseEntity
    {
        [Key]
        [MaxLength(450)]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
