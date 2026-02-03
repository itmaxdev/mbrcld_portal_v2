namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class EliteClubMember : EntityBase
    {
        public string Name { get; set; }
        public string ContactFullName { get; set; }
        public Guid ContactId { get; set; }
        public Guid EliteClubId { get; set; }
        public string PictureUrl { get; set; }

        private EliteClubMember() { }
    }
}