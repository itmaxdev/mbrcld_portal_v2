using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListGroupProjectsViewModel
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public string Topic_Ar { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? PresentationDate { get; set; }
        public int ProjectStatus { get; set; }
        public int Type { get; set; }
        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string AttachmentUrl { get; set; }
        public bool IsParent { get; set; }
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public Guid ProgramId { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Project, ListGroupProjectsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Topic, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Topic_Ar, x => x.MapFrom(src => src.Name_Ar))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Description_Ar, x => x.MapFrom(src => src.Description_Ar))
                    .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                    .ForMember(dst => dst.EndDate, x => x.MapFrom(src => src.EndDate))
                    .ForMember(dst => dst.PresentationDate, x => x.MapFrom(src => src.PresentationDate))
                    .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                    .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.ModuleId))
                    .ForMember(dst => dst.ModuleName, x => x.MapFrom(src => src.ModuleName))
                    .ForMember(dst => dst.TopicId, x => x.MapFrom(src => src.TopicId))
                    .ForMember(dst => dst.TopicName, x => x.MapFrom(src => src.TopicName))
                    .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.ProgramId))
                    .ForMember(dst => dst.AttachmentUrl, x => x.MapFrom(src => src.ProjectAttachmentUrl))
                    .ForMember(dst => dst.IsParent, x => x.MapFrom(src => src.IsParent))
                    .ForMember(dst => dst.ProjectStatus, x => x.MapFrom(src => src.ProjectStatus));
            }
        }
        #endregion
    }
}
