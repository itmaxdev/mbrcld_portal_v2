using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_program")]
    internal sealed class ODataProgram
    {
        [DataMember(Name = "do_programid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_AR { get; set; }

        [DataMember(Name = "do_description")]
        internal string Description { get; set; }

        [DataMember(Name = "do_description_ar")]
        internal string Description_AR { get; set; }

        [DataMember(Name = "do_longdescription")]
        internal string LongDescription { get; set; }

        [DataMember(Name = "do_longdescription_ar")]
        internal string LongDescription_AR { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_startdate")]
        internal DateTime StartDate { get; set; }

        [DataMember(Name = "do_enddate")]
        internal DateTime EndDate { get; set; }

        [DataMember(Name = "do_lastenrollmentdate")]
        internal DateTime LastEnrollmentDate { get; set; }

        [DataMember(Name = "do_order")]
        internal decimal Order { get; set; }

        [DataMember(Name = "do_ActiveCohortId")]
        internal ODataCohort ActiveCohort { get; set; }

        [DataMember(Name = "statuscode")]
        internal int Status { get; set; }

        [DataMember(Name = "do_do_program_do_module")]
        public IList<ODataModule> Module { get; private set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataProgram, Program>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                  .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                  .ForMember(dst => dst.LongDescription, x => x.MapFrom(src => src.LongDescription))
                  .ForMember(dst => dst.LongDescription_AR, x => x.MapFrom(src => src.LongDescription_AR))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                  .ForMember(dst => dst.CohortId, x => x.MapFrom(src => src.ActiveCohort.Id))
                  .ForMember(dst => dst.LastEnrollmentDate, x => x.MapFrom(src => src.LastEnrollmentDate))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
