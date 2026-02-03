using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class Enrollment : EntityBase
    {
        public Guid ContactId { get; set; }
        public Guid ProgramId { get; set; }
        public Guid CohortId { get; set; }
        public int Stage { get; set; }
        public string PymetricsUrl { get; set; }
        public int? PymetricsStatus { get; set; }

        private Enrollment() { }

        public static Enrollment Create(
            Guid contact,
            Guid program,
            Guid cohort
            )
        {
            Guard.Argument(contact, nameof(contact)).NotEqual(default);
            Guard.Argument(program, nameof(program)).NotEqual(default);
            Guard.Argument(cohort, nameof(cohort)).NotEqual(default);

            var enrollment = new Enrollment();
            enrollment.Id = Guid.NewGuid();
            enrollment.ContactId = contact;
            enrollment.ProgramId = program;
            enrollment.CohortId = cohort;
            return enrollment;
        }
    }
}
