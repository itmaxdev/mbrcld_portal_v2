using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetEventByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Duration { get; set; }
        public bool AlreadyRegistered { get; set; }
        public string PictureUrl { get; set; }
        public bool HasQuestions { get; set; }
        public bool AlumniOnly { get; set; }
        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Event, GetEventByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.ToDate, x => x.MapFrom(src => src.ToDate))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                    .ForMember(dst => dst.HasQuestions, x => x.MapFrom(src => src.HasQuestions))
                    .ForMember(dst => dst.AlumniOnly, x => x.MapFrom(src => src.AlumniOnly))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
