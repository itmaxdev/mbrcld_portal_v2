using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetProgramDetailsByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string LongDescription { get; set; }
        public string LongDescription_AR { get; set; }
        public string PictureUrl { get; set; }
        public Guid? EnrollmentId { get; set; }
        public string EnrollmentStatus { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Program, GetProgramDetailsByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                    .ForMember(dst => dst.LongDescription, x => x.MapFrom(src => src.LongDescription))
                    .ForMember(dst => dst.LongDescription_AR, x => x.MapFrom(src => src.LongDescription_AR));
            }
        }
        #endregion
    }
}
