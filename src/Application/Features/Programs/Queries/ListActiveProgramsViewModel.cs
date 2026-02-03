using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListActiveProgramsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public string PictureUrl { get; set; }
        public bool OpenForRegistration { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Program, ListActiveProgramsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR));
            }
        }
        #endregion
    }
}
