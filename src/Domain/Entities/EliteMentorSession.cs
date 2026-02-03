namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class EliteMentorSession : EntityBase
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public Guid MentorId { get; set; }
        public string MentorName { get; set; }
        public Guid ContactId { get; set; }
        public int SessionStatus { get; set; }

        private EliteMentorSession() { }
    }
}