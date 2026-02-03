namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class Calendar : EntityBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int Type { get; set; }
        public Guid ContactID { get; set; }
        public Guid EventID { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingUrl { get; set; }

        private Calendar() { }
    }
}