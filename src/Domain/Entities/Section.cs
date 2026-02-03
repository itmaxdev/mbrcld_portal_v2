namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Section : EntityBase
    {
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public int Duration { get; set; }
        public int? Status { get; set; }
        public decimal Order { get; set; }
        public Guid MaterialId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int SectionStatus { get; set; }

        private Section() { }
        public static Section Create(
            Guid materialid,
            string name,
            string name_ar,
            int duration,
            decimal order,
            DateTime? startdate,
            DateTime? publishdate,
            int sectionstatus
            )
        {
            Guard.Argument(materialid, nameof(materialid)).NotEqual(default);

            var section = new Section();
            section.Id = Guid.NewGuid();
            section.Name = name;
            section.Name_AR = name_ar;
            section.Duration = duration;
            section.Order = order;
            section.MaterialId = materialid;
            section.StartDate = startdate;
            section.PublishDate = publishdate;
            section.SectionStatus = sectionstatus;
            return section;
        }
    }
}