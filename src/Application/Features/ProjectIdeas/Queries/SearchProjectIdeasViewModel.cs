using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class SearchProjectIdeasViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int ProjectIdeaStatus { get; set; }
        public DateTime? Date { get; set; }
        public Guid AlumniId { get; set; }
        public string AlumniName { get; set; }
        public decimal Budget { get; set; }
        public string DocumentUrl { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ProjectIdea, SearchProjectIdeasViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.AlumniId, x => x.MapFrom(src => src.AlumniId))
                    .ForMember(dst => dst.AlumniName, x => x.MapFrom(src => src.AlumniName))
                    .ForMember(dst => dst.ProjectIdeaStatus, x => x.MapFrom(src => src.ProjectIdeaStatus))
                    .ForMember(dst => dst.Budget, x => x.MapFrom(src => src.Budget))
                    .ForMember(dst => dst.Body, x => x.MapFrom(src => src.Body));
            }
        }
        #endregion
    }
}
