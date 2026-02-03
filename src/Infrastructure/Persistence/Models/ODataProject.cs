using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_project")]
    internal sealed class ODataProject
    {
        [DataMember(Name = "do_projectid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_Ar { get; set; }

        [DataMember(Name = "do_description")]
        internal string Desription { get; set; }

        [DataMember(Name = "do_description_ar")]
        internal string Desription_Ar { get; set; }

        [DataMember(Name = "do_projectfile")]
        internal string ProjectFile { get; set; }

        [DataMember(Name = "do_summary")]
        internal string Summary { get; set; }

        [DataMember(Name = "do_rejectionreason")]
        internal string Reason { get; set; }

        [DataMember(Name = "do_approvalreason")]
        internal string ApprovalReason { get; set; }

        [DataMember(Name = "do_InstructorId")]
        internal ODataContact Instructor { get; set; }

        [DataMember(Name = "do_ApplicantId")]
        internal ODataContact Applicant { get; set; }

        [DataMember(Name = "do_ProgramId")]
        internal ODataProgram Program { get; set; }

        [DataMember(Name = "do_ModuleId")]
        internal ODataModule Module { get; set; }

        [DataMember(Name = "do_projectstatus")]
        internal int ProjectStatus { get; set; }

        [DataMember(Name = "do_type")]
        internal int? Type { get; set; }

        [DataMember(Name = "do_adminapproval")]
        internal int? AdminApproval { get; set; }

        [DataMember(Name = "do_template")]
        internal bool Template { get; set; }

        [DataMember(Name = "do_startdate")]
        internal DateTime? StartDate { get; set; }

        [DataMember(Name = "do_enddate")]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "do_presentationdate")]
        public DateTime? PresentationDate { get; set; }

        [DataMember(Name = "do_TopicId")]
        public ODataTopic Topic { get; set; }

        [DataMember(Name = "do_teamlead")]
        public bool TeamLead { get; set; }

        [DataMember(Name = "do_MainProjectId")]
        public ODataProject MainProject { get; set; }

        [DataMember(Name = "do_CohortId")]
        public ODataCohort Cohort { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataProject, Project>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Name_Ar, x => x.MapFrom(src => src.Name_Ar))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Desription))
                  .ForMember(dst => dst.Description_Ar, x => x.MapFrom(src => src.Desription_Ar))
                  .ForMember(dst => dst.InstructorId, x => x.MapFrom(src => src.Instructor.ContactId))
                  .ForMember(dst => dst.ApplicantId, x => x.MapFrom(src => src.Applicant.ContactId))
                  .ForMember(dst => dst.ApplicantName, x => x.MapFrom(src => src.Applicant.FirstName + " " + src.Applicant.MiddleName + " " + src.Applicant.LastName))
                  .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                  .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.Module.Id))
                  .ForMember(dst => dst.ModuleName, x => x.MapFrom(src => src.Module.Name))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                  .ForMember(dst => dst.ProjectStatus, x => x.MapFrom(src => src.ProjectStatus))
                  .ForMember(dst => dst.ProjectAttachmentUrl, x => x.MapFrom(src => src.ProjectFile))
                  .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                  .ForMember(dst => dst.Template, x => x.MapFrom(src => src.Template))
                  .ForMember(dst => dst.AdminApproval, x => x.MapFrom(src => src.AdminApproval))
                  .ForMember(dst => dst.Reason, x => x.MapFrom(src => src.Reason))
                  .ForMember(dst => dst.Summary, x => x.MapFrom(src => src.Summary))
                  .ForMember(dst => dst.ApprovalReason, x => x.MapFrom(src => src.ApprovalReason))
                  .ForMember(dst => dst.TopicId, x => x.MapFrom(src => src.Topic.TopicId))
                  .ForMember(dst => dst.TopicName, x => x.MapFrom(src => src.Topic.TopicName))
                  .ForMember(dst => dst.MainProjectId, x => x.MapFrom(src => src.MainProject.Id))
                  .ForMember(dst => dst.TeamLead, x => x.MapFrom(src => src.TeamLead))
                  .ForMember(dst => dst.PresentationDate, x => x.MapFrom(src => src.PresentationDate))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
