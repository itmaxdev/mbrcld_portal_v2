using Mbrcld.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbrcld.Domain.Entities
{
    public sealed class EventQuestion : EntityBase
    {
        public Guid EventId { get; set; }
        /*public Guid CohortId { get; set; }*/
        public string Name { get; set; }

        private EventQuestion() { }
    }
}
