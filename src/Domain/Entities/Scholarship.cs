using Dawn;
using Mbrcld.Domain.Common;
using System;
using System.Collections.Generic;

namespace Mbrcld.Domain.Entities
{
    public class Scholarship : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool OpenForRegistration { get; set; }
        public bool AlreadyRegistered { get; set; }
        public ScholarshipRegistrationStatus StatusCode { get; set; }
        public IReadOnlyList<ScholarshipRegistration> ScholarshipRegistrations { get; private set; }

        public Scholarship() { }
    }
    public enum ScholarshipRegistrationStatus
    {
        UnderReview = 1,
        Accepted = 936510000,
        Rejected = 936510001
    }
}
