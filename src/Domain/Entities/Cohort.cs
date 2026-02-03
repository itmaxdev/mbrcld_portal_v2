namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Cohort : EntityBase
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int? Year { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastEnrollmentDate { get; set; }
        public bool OpenForRegistration { get; set; }
        public Guid ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramName_AR { get; set; }
        public string ProgramDesription { get; set; }
        public string ProgramDesription_AR { get; set; }
        public decimal TotalCost { get; set; }

        private Cohort() { }
    }
}