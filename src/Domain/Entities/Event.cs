namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class Event : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Duration { get; set; }
        public int MaxCapacity { get; set; }
        public bool AlreadyRegistered { get; set; }
        public bool HasQuestions { get; set; }
        public bool AlumniOnly { get; set; }
        
        public IReadOnlyList<EventRegistrant> EventRegistrants { get; private set; }

        private Event() { }
    }
}