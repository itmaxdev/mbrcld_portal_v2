using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetApplicantProfileViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string BusinessEmail { get; set; }
        public string Nationality { get; set; }
        public string ResidenceCountry { get; set; }
        public string LinkedInUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string JobTitle { get; set; }
        public string Organization { get; set; }
        public string Education { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, GetApplicantProfileViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.FirstName + " " + src.MiddleName + " " + src.LastName))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.Mobile, x => x.MapFrom(src => src.MobilePhone))
                    .ForMember(dst => dst.BusinessEmail, x => x.MapFrom(src => src.BusinessEmail))
                    .ForMember(dst => dst.Nationality, x => x.MapFrom(src => src.Nationality.Name))
                    .ForMember(dst => dst.ResidenceCountry, x => x.MapFrom(src => src.ResidenceCountry.Name))
                    .ForMember(dst => dst.LinkedInUrl, x => x.MapFrom(src => src.LinkedInUrl))
                    .ForMember(dst => dst.ProfilePictureUrl, x => x.MapFrom(src => src.ProfilePictureUniqueId));
            }
        }
        #endregion
    }
}
