namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class Module : EntityBase
    {
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public string Location { get; set; }
        public string ModuleUrl { get; set; }
        public string Overview { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Order { get; set; }
        public Guid ProgramId { get; set; }
        public Guid EliteClubId { get; set; }
        public Guid CohortId { get; set; }
        public string CohortName { get; set; }
        public Guid InstructorId { get; set; }
        public decimal? Completed { get; set; }
        public IReadOnlyList<ModuleCompletion> ModuleCompletions { get; private set; }

        private Module() { }
    }
}