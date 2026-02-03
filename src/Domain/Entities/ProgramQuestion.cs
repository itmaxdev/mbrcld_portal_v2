using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class ProgramQuestion : EntityBase
    {       
        public Guid Program { get; set; }       
        public Guid CohortId { get; set; }       
        public string Name { get; set; }

        private ProgramQuestion() { }
    }
}
