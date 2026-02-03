namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Project : EntityBase
    {
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public Guid? ProgramId { get; set; }
        public Guid? ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public Guid? InstructorId { get; set; }
        public Guid? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Summary { get; set; }
        public string Reason { get; set; }
        public string ApprovalReason { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PresentationDate { get; set; }
        public int? AdminApproval { get; set; }
        public int? ProjectStatus { get; set; }
        public int Type { get; set; }
        public bool Template { get; set; }
        public string ProjectAttachmentUrl { get; set; }
        public Guid? TopicId { get; set; }
        public string TopicName { get; set; }
        public Guid? MainProjectId { get; set; }
        public bool TeamLead { get; set; }
        public bool IsParent { get; set; }

        private Project() { }
        public static Project Create(
            string name,
            string name_ar,
            string description,
            string description_ar,
            Guid? programId,
            Guid? applicantId,
            Guid? instructorId,
            Guid? moduleId,
            DateTime? startdate,
            DateTime? enddate,
            int projectstatus,
            int type,
            bool template
            )
        {
            Guard.Argument(name, nameof(name)).NotEqual(default).NotNull();

            var project = new Project();
            project.Id = Guid.NewGuid();
            project.Name = name;
            project.Name_Ar = name_ar;
            project.Description = description;
            project.Description_Ar = description_ar;
            project.ProgramId = programId;
            project.ModuleId = moduleId;
            project.ApplicantId = applicantId;
            project.InstructorId = instructorId;
            project.StartDate = startdate;
            project.EndDate = enddate;
            project.ProjectStatus = projectstatus;
            project.Type = type;
            project.Template = template;
            return project;
        }
    }
}