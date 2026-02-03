using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_module")]
    internal sealed class ODataModule
    {
        [DataMember(Name = "do_moduleid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_AR { get; set; }

        [DataMember(Name = "do_description")]
        internal string Description { get; set; }

        [DataMember(Name = "do_description_ar")]
        internal string Description_AR { get; set; }

        [DataMember(Name = "do_location")]
        internal string Location { get; set; }

        [DataMember(Name = "do_moduleurl")]
        internal string ModuleUrl { get; set; }

        [DataMember(Name = "do_overview")]
        internal string Overview { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_startdate")]
        internal DateTime StartDate { get; set; }

        [DataMember(Name = "do_enddate")]
        internal DateTime EndDate { get; set; }

        [DataMember(Name = "do_order")]
        internal decimal Order { get; set; }

        [DataMember(Name = "do_ProgramId")]
        internal ODataProgram Program { get; set; }

        [DataMember(Name = "do_EliteClubId")]
        internal ODataEliteClub EliteClub { get; set; }

        [DataMember(Name = "do_CohortId")]
        internal ODataCohort Cohort { get; set; }

        [DataMember(Name = "do_InstructorId")]
        internal ODataContact Instructor { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataModule, Module>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                  .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                  .ForMember(dst => dst.Location, x => x.MapFrom(src => src.Location))
                  .ForMember(dst => dst.ModuleUrl, x => x.MapFrom(src => src.ModuleUrl))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                  .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                  .ForMember(dst => dst.CohortName, x => x.MapFrom(src => src.Cohort.Name))
                  .ForMember(dst => dst.EliteClubId, x => x.MapFrom(src => src.EliteClub.Id))
                  .ForMember(dst => dst.CohortId, x => x.MapFrom(src => src.Cohort.Id))
                  .ForMember(dst => dst.InstructorId, x => x.MapFrom(src => src.Instructor.ContactId))
                  .ForMember(dst => dst.Overview, x => x.MapFrom(src => src.Overview))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
