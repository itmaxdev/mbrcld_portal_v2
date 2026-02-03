namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class SectionCompletion : EntityBase
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public Guid SectionId { get; set; }
        public Guid ContactID { get; set; }

        private SectionCompletion() { }
    }
}