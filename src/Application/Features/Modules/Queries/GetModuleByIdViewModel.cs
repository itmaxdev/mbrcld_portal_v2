using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetModuleByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid InstructorId { get; set; }
        public Guid ProgramId { get; set; }
        public string Location { get; set; }
        public string ModuleUrl { get; set; }
        public string Overview { get; set; }
        public decimal Order { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Module, GetModuleByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Description_AR, x => x.MapFrom(src => src.Description_AR))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                    .ForMember(dst => dst.Location, x => x.MapFrom(src => src.Location))
                    .ForMember(dst => dst.ModuleUrl, x => x.MapFrom(src => src.ModuleUrl))
                    .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                    .ForMember(dst => dst.InstructorId, x => x.MapFrom(src => src.InstructorId))
                    .ForMember(dst => dst.Overview, x => x.MapFrom(src => src.Overview))
                    .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.ProgramId));
            }
        }
        #endregion
    }
}
