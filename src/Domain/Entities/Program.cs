namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class Program : EntityBase
    {
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public string LongDescription { get; set; }
        public string LongDescription_AR { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? LastEnrollmentDate { get; set; }
        public decimal Order { get; set; }
        public decimal Completed { get; set; }
        public Guid CohortId { get; set; }
        public bool OpenForRegistration { get; set; }
        public IReadOnlyList<Module> ProgramModules { get; private set; }

        private Program() { }
    }
}