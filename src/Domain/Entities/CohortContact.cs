namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class CohortContact : EntityBase
    {
        public string Name { get; set; }
        public decimal Completed { get; set; }
        public int Status { get; set; }
        public Guid ProgramId { get; set; }
        public Guid ContactID { get; set; }
        public Guid CohortID { get; set; }

        private CohortContact() { }
    }
}