using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_enrollment")]
    internal sealed class ODataEnrollment
    {
        [DataMember(Name = "do_enrollmentid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_ProgramId")]
        internal ODataProgram Program { get; set; }

        [DataMember(Name = "do_CohortId")]
        internal ODataCohort Cohort { get; set; }

        [DataMember(Name = "do_pymetricsassessmenturl")]
        internal string PymetricsAssessmentUrl { get; set; }

        [DataMember(Name = "do_pymetricsstatus")]
        internal int? PymetricsStatus { get; set; }

        [DataMember(Name = "do_stage")]
        internal int Stage { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEnrollment, Enrollment>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                  .ForMember(dst => dst.CohortId, x => x.MapFrom(src => src.Cohort.Id))
                  .ForMember(dst => dst.PymetricsUrl, x => x.MapFrom(src => src.PymetricsAssessmentUrl))
                  .ForMember(dst => dst.PymetricsStatus, x => x.MapFrom(src => src.PymetricsStatus))
                  .ForMember(dst => dst.Stage, x => x.MapFrom(src => src.Stage))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
