namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Material : EntityBase
    {
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public decimal Order { get; set; }
        public Guid ModuleId { get; set; }
        public decimal? Completed { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int Status { get; set; }

        private Material() { }
        public static Material Create(
            Guid moduleid,
            string name,
            string name_ar,
            string location,
            int duration,
            decimal order,
            DateTime? startdate,
            DateTime? publishdate,
            int status
            )
        {
            Guard.Argument(moduleid, nameof(moduleid)).NotEqual(default);

            var material = new Material();
            material.Id = Guid.NewGuid();
            material.Name = name;
            material.Name_AR = name_ar;
            material.Location = location;
            material.Duration = duration;
            material.Order = order;
            material.StartDate = startdate;
            material.ModuleId = moduleid;
            material.PublishDate = publishdate;
            material.Status = status;
            return material;
        }
    }
}