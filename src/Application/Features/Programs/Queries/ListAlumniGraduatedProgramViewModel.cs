using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListAlumniGraduatedProgramViewModel
    {
        public Guid ProgramId { get; set; }
        public Guid CohortId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramName_AR { get; set; }
        public string ProgramDescription{ get; set; }
        public string ProgramDescription_AR { get; set; }
        public int? CohortYear { get; set; }
        public string PictureUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Cohort, ListAlumniGraduatedProgramViewModel>()
                    .ForMember(dst => dst.CohortId, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.ProgramId))
                    .ForMember(dst => dst.ProgramName, x => x.MapFrom(src => src.ProgramName))
                    .ForMember(dst => dst.ProgramName, x => x.MapFrom(src => src.ProgramName_AR))
                    .ForMember(dst => dst.ProgramDescription, x => x.MapFrom(src => src.ProgramDesription))
                    .ForMember(dst => dst.ProgramDescription_AR, x => x.MapFrom(src => src.ProgramDesription_AR))
                    .ForMember(dst => dst.CohortYear, x => x.MapFrom(src => src.Year))
                    .ReverseMap();
            }
        }
        #endregion
    }
}

