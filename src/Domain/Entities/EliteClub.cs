namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class EliteClub : EntityBase
    {
        public string Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Overview { get; set; }

        private EliteClub() { }
    }
}