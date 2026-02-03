namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class EliteMembershipAttendance : EntityBase
    {
        public string Name { get; set; }
        public Guid ContactId { get; set; }
        public Guid EliteMembeshipId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public int AttendanceStatus { get; set; }

        private EliteMembershipAttendance() { }
    }
}