using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public class ScholarshipRegistration : EntityBase
    {
        public string Name { get; set; }
        public Guid ScholarshipId { get; set; }
        public Guid UserId { get; set; }
        public ScholarshipRegistrationStatus StatusCode { get; set; }

        public ScholarshipRegistration() { }
        public static ScholarshipRegistration Create(
           Guid userid,
           Guid scholarshipId
           )
        {
            Guard.Argument(userid, nameof(userid)).NotEqual(default);
            Guard.Argument(scholarshipId, nameof(scholarshipId)).NotEqual(default);

            var scholarshipRegistration = new ScholarshipRegistration();
            scholarshipRegistration.Id = Guid.NewGuid();
            scholarshipRegistration.UserId = userid;
            scholarshipRegistration.ScholarshipId = scholarshipId;
            scholarshipRegistration.StatusCode = ScholarshipRegistrationStatus.UnderReview;
            return scholarshipRegistration;
        }

        public enum ScholarshipRegistrationStatus
        {
            UnderReview = 1,
            Accepted = 936510000,
            Rejected = 936510001
        }
    }
}
