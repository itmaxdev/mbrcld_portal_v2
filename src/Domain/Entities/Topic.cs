namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class Topic : EntityBase
    {
        public string TopicName { get; set; }
        public int MaxCount { get; set; }
        public Guid ProgramId { get; set; }
        public Guid CohortId { get; set; }

        private Topic() { }
    }
}