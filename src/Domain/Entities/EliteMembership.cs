namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class EliteMembership : EntityBase
    {
        public string Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public Guid EliteClubId { get; set; }

        private EliteMembership() { }
    }
}