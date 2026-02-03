namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class SpecialProjectTopic : EntityBase
    {
        public string SpecialProjectTopicName { get; set; }

        private SpecialProjectTopic() { }
    }
}