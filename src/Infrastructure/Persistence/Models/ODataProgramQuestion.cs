using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_programquestion")]
    internal sealed class ODataProgramQuestion
    {
        [DataMember(Name = "do_programquestionid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        public string Name_AR { get; set; }

        [DataMember(Name = "do_programid")]
        internal ODataProgram ProgramId { get; set; }

        [DataMember(Name = "do_CohortId")]
        internal ODataCohort CohortId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataProgramQuestion, ProgramQuestion>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Program, x => x.MapFrom(src => src.ProgramId.Id))
                  .ForMember(dst => dst.CohortId, x => x.MapFrom(src => src.CohortId.Id));
            }
        }
        #endregion
    }
}

