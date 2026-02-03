using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetUniversityByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AboutUniversity { get; set; }
        public Guid? AccessInstructorId { get; set; }
        public string AccessInstructorName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string POBox { get; set; }
        public string Twitter { get; set; }
        public string Website { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<University, GetUniversityByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Phone, x => x.MapFrom(src => src.Phone))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.AboutUniversity, x => x.MapFrom(src => src.AboutUniversity))
                    .ForMember(dst => dst.AccessInstructorName, x => x.MapFrom(src => src.AccessInstructorName))
                    .ForMember(dst => dst.City, x => x.MapFrom(src => src.City))
                    .ForMember(dst => dst.Country, x => x.MapFrom(src => src.Country.IsoCode))
                    .ForMember(dst => dst.Address, x => x.MapFrom(src => src.Address))
                    .ForMember(dst => dst.Instagram, x => x.MapFrom(src => src.Instagram))
                    .ForMember(dst => dst.LinkedIn, x => x.MapFrom(src => src.LinkedIn))
                    .ForMember(dst => dst.POBox, x => x.MapFrom(src => src.POBox))
                    .ForMember(dst => dst.Twitter, x => x.MapFrom(src => src.Twitter))
                    .ForMember(dst => dst.Website, x => x.MapFrom(src => src.Website))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
