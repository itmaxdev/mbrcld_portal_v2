using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_cohort")]
    internal sealed class ODataCohort
    {
        [DataMember(Name = "do_cohortid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_startdate")]
        internal DateTime? StartDate { get; set; }

        [DataMember(Name = "do_enddate")]
        internal DateTime? EndDate { get; set; }

        [DataMember(Name = "do_lastenrollmentdate")]
        internal DateTime? LastEnrollmentDate { get; set; }

        [DataMember(Name = "do_openforregistration")]
        internal bool OpenForRegistration { get; set; }

        [DataMember(Name = "do_totalcost")]
        internal decimal TotalCost { get; set; }

        [DataMember(Name = "do_year")]
        internal int? Year { get; set; }

        [DataMember(Name = "do_ProgramId")]
        internal ODataProgram Program { get; set; }

        
        //To Add Cohort

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataCohort, Cohort>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                  .ForMember(dst => dst.ProgramDesription, x => x.MapFrom(src => src.Program.Description))
                  .ForMember(dst => dst.ProgramDesription_AR, x => x.MapFrom(src => src.Program.Description_AR))
                  .ForMember(dst => dst.ProgramName, x => x.MapFrom(src => src.Program.Name))
                  .ForMember(dst => dst.ProgramName_AR, x => x.MapFrom(src => src.Program.Name_AR))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                  .ForMember(dst => dst.LastEnrollmentDate, x => x.MapFrom(src => src.LastEnrollmentDate))
                  .ForMember(dst => dst.OpenForRegistration, x => x.MapFrom(src => src.OpenForRegistration))
                  .ForMember(dst => dst.TotalCost, x => x.MapFrom(src => src.TotalCost))
                  .ForMember(dst => dst.Year, x => x.MapFrom(src => src.Year))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
