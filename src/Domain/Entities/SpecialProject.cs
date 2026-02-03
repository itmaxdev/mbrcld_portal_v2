namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class SpecialProject : EntityBase
    {
        public string Name { get; set; }
        public int SpecialProjectStatus { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Guid AlumniId { get; set; }
        public string AlumniName { get; set; }
        public string Benchmark { get; set; }
        public decimal Budget { get; set; }
        public DateTime? Date { get; set; }
        public Guid SpecialProjectTopicId{ get; set; }
        public Guid SectorId{ get; set; }
        public string OtherSector { get; set; }

        private SpecialProject() { }
        public static SpecialProject Create(
            Guid userid,
            string description,
            string body,
            DateTime? date,
            int specialprojectstatus,
            string name,
            string benchmark,
            decimal budget,
            Guid specialprojecttopicid,
            Guid sectorid,
            string otherSector
            )
        {
            Guard.Argument(userid, nameof(userid)).NotEqual(default);

            var specialProject = new SpecialProject();
            specialProject.Id = Guid.NewGuid();
            specialProject.AlumniId = userid;
            specialProject.Description = description;
            specialProject.Body = body;
            specialProject.SpecialProjectStatus = specialprojectstatus;
            specialProject.Date = date;
            specialProject.Name = name;
            specialProject.Budget = budget;
            specialProject.Benchmark = benchmark;
            specialProject.SpecialProjectTopicId = specialprojecttopicid;
            specialProject.SectorId = sectorid;
            specialProject.OtherSector = otherSector;
            return specialProject;
        }
    }
}