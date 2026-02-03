using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetMentorByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AboutMentor { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string NationalityName { get; set; }
        public string Education { get; set; }
        public string JobPosition { get; set; }
        public string ResidenceCountry { get; set; }
        public string ResidenceCountryName { get; set; }
        public string PictureUrl { get; set; }
        public string LinkedIn { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Mentor, GetMentorByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.Education, x => x.MapFrom(src => src.Education))
                    .ForMember(dst => dst.JobPosition, x => x.MapFrom(src => src.JobPosition))
                    .ForMember(dst => dst.LinkedIn, x => x.MapFrom(src => src.LinkedIn))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.AboutMentor, x => x.MapFrom(src => src.AboutMentor))
                    .ForMember(dst => dst.Nationality, x => x.MapFrom(src => src.Nationality.IsoCode))
                    .ForMember(dst => dst.NationalityName, x => x.MapFrom(src => src.Nationality.Name))
                    .ForMember(dst => dst.ResidenceCountry, x => x.MapFrom(src => src.ResidenceCountry.IsoCode))
                    .ForMember(dst => dst.ResidenceCountryName, x => x.MapFrom(src => src.ResidenceCountry.Name));
            }
        }
        #endregion
    }
}
