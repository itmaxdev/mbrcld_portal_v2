namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class ProjectIdea : EntityBase
    {
        public string Name { get; set; }
        public int ProjectIdeaStatus { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Guid AlumniId { get; set; }
        public string AlumniName { get; set; }
        public string Benchmark { get; set; }
        public DateTime? Date { get; set; }
        public decimal Budget { get; set; }
        public Guid SectorId { get; set; }
        public string OtherSector { get; set; }

        private ProjectIdea() { }
        public static ProjectIdea Create(
            Guid userid,
            string description,
            string body,
            DateTime? date,
            int projectideastatus,
            string name,
            decimal budget,
            string benchmark,
            Guid sectorid,
            string otherSector
            )
        {
            Guard.Argument(userid, nameof(userid)).NotEqual(default);

            var projectidea = new ProjectIdea();
            projectidea.Id = Guid.NewGuid();
            projectidea.AlumniId = userid;
            projectidea.Description = description;
            projectidea.Body = body;
            projectidea.ProjectIdeaStatus = projectideastatus;
            projectidea.Date = date;
            projectidea.Name = name;
            projectidea.Budget = budget;
            projectidea.Benchmark = benchmark;
            projectidea.SectorId = sectorid;
            projectidea.OtherSector = otherSector;
            return projectidea;
        }
    }
}