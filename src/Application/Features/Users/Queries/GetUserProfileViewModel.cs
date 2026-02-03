using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserProfileViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string FirstName_AR { get; set; }
        public string MiddleName { get; set; }
        public string MiddleName_AR { get; set; }
        public string LastName { get; set; }
        public string LastName_AR { get; set; }
        public int? Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Email { get; set; }
        public string BusinessEmail { get; set; }
        public string MobilePhone { get; set; }
        public string MobilePhone2 { get; set; }
        public string Telephone { get; set; }
        public string ResidenceCountry { get; set; }
        public string City { get; set; }
        public string PostOfficeBox { get; set; }
        public string Address { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string EmiratesId { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int? MaritalStatus { get; set; }
        public int? Salutation { get; set; }
        public DateTime? EmiratesIdExpiryDate { get; set; }
        public int? PassportIssuingAuthority { get; set; }
        public bool? IsActiveMember { get; set; }
        public int Role { get; set; }
        public string AboutInstructor { get; set; }
        public string RepresentedUniversityIntroduction { get; set; }
        public DateTime? ProfileUpdateOn { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, GetUserProfileViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.FirstName, x => x.MapFrom(src => src.FirstName))
                    .ForMember(dst => dst.FirstName_AR, x => x.MapFrom(src => src.FirstNameAr))
                    .ForMember(dst => dst.MiddleName, x => x.MapFrom(src => src.MiddleName))
                    .ForMember(dst => dst.MiddleName_AR, x => x.MapFrom(src => src.MiddleNameAr))
                    .ForMember(dst => dst.LastName, x => x.MapFrom(src => src.LastName))
                    .ForMember(dst => dst.LastName_AR, x => x.MapFrom(src => src.LastNameAr))
                    .ForMember(dst => dst.Gender, x => x.MapFrom(src => src.Gender))
                    .ForMember(dst => dst.Nationality, x => x.MapFrom(src => src.Nationality == null ? null : src.Nationality.IsoCode))
                    .ForMember(dst => dst.Birthdate, x => x.MapFrom(src => src.BirthDate))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.Email))
                    .ForMember(dst => dst.BusinessEmail, x => x.MapFrom(src => src.BusinessEmail))
                    .ForMember(dst => dst.MobilePhone, x => x.MapFrom(src => src.MobilePhone))
                    .ForMember(dst => dst.MobilePhone2, x => x.MapFrom(src => src.MobilePhone2))
                    .ForMember(dst => dst.Telephone, x => x.MapFrom(src => src.Telephone))
                    .ForMember(dst => dst.ResidenceCountry, x => x.MapFrom(src => src.ResidenceCountry == null ? null : src.ResidenceCountry.IsoCode))
                    .ForMember(dst => dst.City, x => x.MapFrom(src => src.City))
                    .ForMember(dst => dst.PostOfficeBox, x => x.MapFrom(src => src.PostOfficeBox))
                    .ForMember(dst => dst.Address, x => x.MapFrom(src => src.Address))
                    .ForMember(dst => dst.LinkedInUrl, x => x.MapFrom(src => src.LinkedInUrl))
                    .ForMember(dst => dst.InstagramUrl, x => x.MapFrom(src => src.InstagramUrl))
                    .ForMember(dst => dst.TwitterUrl, x => x.MapFrom(src => src.TwitterUrl))
                    .ForMember(dst => dst.EmiratesId, x => x.MapFrom(src => src.EmiratesId))
                    .ForMember(dst => dst.PassportNumber, x => x.MapFrom(src => src.PassportNumber))
                    .ForMember(dst => dst.PassportExpiryDate, x => x.MapFrom(src => src.PassportExpiryDate))
                    .ForMember(dst => dst.ProfilePictureUrl, x => x.MapFrom(src => src.ProfilePictureUniqueId))
                    .ForMember(dst => dst.MaritalStatus, x => x.MapFrom(src => src.MaritalStatus))
                    .ForMember(dst => dst.EmiratesIdExpiryDate, x => x.MapFrom(src => src.EmiratesIdExpiryDate))
                    .ForMember(dst => dst.PassportIssuingAuthority, x => x.MapFrom(src => src.PassportIssuingAuthority))
                    .ForMember(dst => dst.Role, x => x.MapFrom(src => src.Role))
                    .ForMember(dst => dst.AboutInstructor, x => x.MapFrom(src => src.AboutInstructor))
                    .ForMember(dst => dst.RepresentedUniversityIntroduction, x => x.MapFrom(src => src.RepresentedUniversityIntroduction))
                    .ForMember(dst => dst.IsActiveMember, x => x.MapFrom(src => src.IsActiveMemberInBoardOrInstitution))
                    .ForMember(dst => dst.ProfileUpdateOn, x => x.MapFrom(src => src.ProfileUpdateOn));
            }
        }
        #endregion
    }
}
