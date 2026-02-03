using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListProjectsByCohortIdViewModel
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public string Topic_Ar { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectStatus { get; set; }
        public int Type { get; set; }
        public string AttachmentUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Project, ListProjectsByCohortIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Topic, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Topic_Ar, x => x.MapFrom(src => src.Name_Ar))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Description_Ar, x => x.MapFrom(src => src.Description_Ar))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                    .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                    .ForMember(dst => dst.AttachmentUrl, x => x.MapFrom(src => src.ProjectAttachmentUrl))
                    .ForMember(dst => dst.ProjectStatus, x => x.MapFrom(src => src.ProjectStatus));
            }
        }
        #endregion
    }
}
