using AutoMapper;
using Mbrcld.Domain.Entities;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "contacts")]
    internal sealed class ODataContact
    {
        [DataMember(Name = "contactid")]
        internal Guid ContactId { get; set; }

        [DataMember(Name = "firstname")]
        internal string FirstName { get; set; }

        [DataMember(Name = "do_firstname_ar")]
        internal string FirstName_Ar { get; set; }

        [DataMember(Name = "middlename")]
        internal string MiddleName { get; set; }

        [DataMember(Name = "do_middlename_ar")]
        internal string MiddleName_Ar { get; set; }

        [DataMember(Name = "lastname")]
        internal string LastName { get; set; }

        [DataMember(Name = "do_lastname_ar")]
        internal string LastName_Ar { get; set; }

        [DataMember(Name = "emailaddress1")]
        internal string EmailAddress1 { get; set; }

        [DataMember(Name = "emailaddress2")]
        internal string EmailAddress2 { get; set; }

        [DataMember(Name = "gendercode")]
        internal int? Gender { get; set; }

        [DataMember(Name = "mobilephone")]
        internal string MobilePhone { get; set; }

        [DataMember(Name = "telephone3")]
        internal string MobilePhone2 { get; set; }

        [DataMember(Name = "telephone2")]
        internal string Telephone { get; set; }

        [DataMember(Name = "do_NationalityId")]
        internal ODataCountry Nationality { get; set; }

        [DataMember(Name = "do_ResidenceCountryId")]
        internal ODataCountry ResidenceCountry { get; set; }

        [DataMember(Name = "birthdate")]
        internal Date? BirthDate { get; set; }

        [DataMember(Name = "address1_city")]
        internal string City { get; set; }

        [DataMember(Name = "address1_postofficebox")]
        internal string PostOfficeBox { get; set; }

        [DataMember(Name = "do_address")]
        internal string Address { get; set; }

        [DataMember(Name = "do_linkedinurl")]
        internal string LinkedInUrl { get; set; }

        [DataMember(Name = "do_instagramurl")]
        internal string InstagramUrl { get; set; }

        [DataMember(Name = "do_twitterurl")]
        internal string TwitterUrl { get; set; }

        [DataMember(Name = "do_uaeemiratesid")]
        internal string EmiratesId { get; set; }

        [DataMember(Name = "do_passportnumber")]
        internal string PassportNumber { get; set; }

        [DataMember(Name = "do_passportexpirydate")]
        internal DateTime? PassportExpiryDate { get; set; }

        [DataMember(Name = "do_learningpreferences")]
        internal string LearningPreferences { get; set; }

        [DataMember(Name = "do_entityimagekey")]
        internal string EntityImageKey { get; set; }

        [DataMember(Name = "familystatuscode")]
        internal int? MaritalStatus { get; set; }       

        [DataMember(Name = "do_uaeemiratesidexpirydate")]
        internal DateTime? EmiratesIdExpiryDate { get; set; }

        [DataMember(Name = "do_passportissuingauthority")]
        internal int? PassportIssuingAuthority { get; set; }

        [DataMember(Name = "do_isactivemember")]
        internal int? IsActiveMember { get; set; }

        [DataMember(Name = "do_role")]
        internal int Role { get; set; }

        [DataMember(Name = "do_aboutinstructor")]
        internal string AboutInstructor { get; set; }

        [DataMember(Name = "do_representeduniversityintroduction")]
        internal string RepresentedUniversityIntroduction { get; set; }

        [DataMember(Name = "do_UniversityId")]
        internal ODataUniversity University { get; set; }

        [DataMember(Name = "do_DirectManagerId")]
        internal ODataContact DirectManager { get; set; }

        [DataMember(Name = "do_articlesdisclaimer")]
        internal bool ArticlesDisclaimer { get; set; }

        [DataMember(Name = "do_ideahubdisclaimer")]
        internal bool IdeaHubDisclaimer { get; set; }

        [DataMember(Name = "do_specialprojectdisclaimer")]
        internal bool SpecialProjectDisclaimer { get; set; }

        [DataMember(Name = "do_deleteaccount")]
        internal int DeleteAccount { get; set; }
        

        //[DataMember(Name = "do_LastProfileUpdateOn")]
        [DataMember(Name = "do_lastprofileupdateon")]
        internal DateTime? ProfileUpdateOn { get; set; }

        #region Relationships
        [DataMember(Name = "Contact_Annotation")]
        public IList<ODataAnnotation> Documents { get; private set; }

        [DataMember(Name = "do_contact_do_achievement")]
        public IList<ODataAchievement> Achievements { get; private set; }

        [DataMember(Name = "do_contact_do_educationqualification")]
        public IList<ODataEducationQualification> EducationQualifications { get; private set; }

        [DataMember(Name = "do_contact_do_skillandinterest")]
        public IList<ODataSkillAndInterest> Interests { get; private set; }

        [DataMember(Name = "do_contact_do_languagelevel")]
        public IList<ODataLanguageSkill> LanguageSkills { get; private set; }

        [DataMember(Name = "do_contact_do_membership")]
        public IList<ODataMembership> Memberships { get; private set; }

        [DataMember(Name = "do_contact_do_professionalexperience")]
        internal IList<ODataProfessionalExperience> ProfessionalExperiences { get; set; }

        [DataMember(Name = "do_contact_do_reference")]
        public IList<ODataReference> References { get; private set; }

        [DataMember(Name = "do_contact_do_trainingcourse")]
        public IList<ODataTrainingCourse> TrainingCourses { get; private set; }

        #endregion 

        #region Identity
        [DataMember(Name = "do_password")]
        internal string PasswordHash { get; set; }

        [DataMember(Name = "do_isemailconfirmed")]
        internal bool EmailConfirmed { get; set; }

        [DataMember(Name = "do_securitystamp")]
        internal string SecurityStamp { get; set; }

        #endregion

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataContact, User>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.ContactId))
                    .ForMember(dst => dst.FirstName, x => x.MapFrom(src => src.FirstName))
                    .ForMember(dst => dst.FirstNameAr, x => x.MapFrom(src => src.FirstName_Ar))
                    .ForMember(dst => dst.MiddleName, x => x.MapFrom(src => src.MiddleName))
                    .ForMember(dst => dst.MiddleNameAr, x => x.MapFrom(src => src.MiddleName_Ar))
                    .ForMember(dst => dst.LastName, x => x.MapFrom(src => src.LastName))
                    .ForMember(dst => dst.LastNameAr, x => x.MapFrom(src => src.LastName_Ar))
                    .ForMember(dst => dst.Email, x => x.MapFrom(src => src.EmailAddress1))
                    .ForMember(dst => dst.BusinessEmail, x => x.MapFrom(src => src.EmailAddress2))
                    .ForMember(dst => dst.Gender, x => x.MapFrom(src => src.Gender))
                    .ForMember(dst => dst.MaritalStatus, x => x.MapFrom(src => src.MaritalStatus))
                    .ForMember(dst => dst.MobilePhone, x => x.MapFrom(src => src.MobilePhone))
                    .ForMember(dst => dst.MobilePhone2, x => x.MapFrom(src => src.MobilePhone2))
                    .ForMember(dst => dst.Telephone, x => x.MapFrom(src => src.Telephone))
                    .ForMember(dst => dst.Nationality, x => x.MapFrom(src => src.Nationality))
                    .ForMember(dst => dst.ResidenceCountry, x => x.MapFrom(src => src.ResidenceCountry))
                    .ForMember(dst => dst.BirthDate, x => x.MapFrom(src => (DateTime?)src.BirthDate))
                    .ForMember(dst => dst.City, x => x.MapFrom(src => src.City))
                    .ForMember(dst => dst.PostOfficeBox, x => x.MapFrom(src => src.PostOfficeBox))
                    .ForMember(dst => dst.Address, x => x.MapFrom(src => src.Address))
                    .ForMember(dst => dst.LinkedInUrl, x => x.MapFrom(src => src.LinkedInUrl))
                    .ForMember(dst => dst.InstagramUrl, x => x.MapFrom(src => src.InstagramUrl))
                    .ForMember(dst => dst.TwitterUrl, x => x.MapFrom(src => src.TwitterUrl))
                    .ForMember(dst => dst.EmiratesId, x => x.MapFrom(src => src.EmiratesId))
                    .ForMember(dst => dst.EmiratesIdExpiryDate, x => x.MapFrom(src => src.EmiratesIdExpiryDate))
                    .ForMember(dst => dst.PassportNumber, x => x.MapFrom(src => src.PassportNumber))
                    .ForMember(dst => dst.PassportExpiryDate, x => x.MapFrom(src => src.PassportExpiryDate))
                    .ForMember(dst => dst.PassportIssuingAuthority, x => x.MapFrom(src => src.PassportIssuingAuthority))
                    .ForMember(dst => dst.LearningPreferences, x => x.MapFrom(src => src.LearningPreferences == null ? null : src.LearningPreferences.Split(",", StringSplitOptions.None).Select(int.Parse)))
                    .ForMember(dst => dst.Achievements, x => x.MapFrom(src => src.Achievements))
                    .ForMember(dst => dst.EducationQualifications, x => x.MapFrom(src => src.EducationQualifications))
                    .ForMember(dst => dst.Interests, x => x.MapFrom(src => src.Interests))
                    .ForMember(dst => dst.LanguageSkills, x => x.MapFrom(src => src.LanguageSkills))
                    .ForMember(dst => dst.IsActiveMemberInBoardOrInstitution, x => x.MapFrom(src => src.IsActiveMember == null ? (bool?)null : src.IsActiveMember == 1))
                    .ForMember(dst => dst.Memberships, x => x.MapFrom(src => src.Memberships))
                    .ForMember(dst => dst.ProfessionalExperiences, x => x.MapFrom(src => src.ProfessionalExperiences))
                    .ForMember(dst => dst.References, x => x.MapFrom(src => src.References))
                    .ForMember(dst => dst.TrainingCourses, x => x.MapFrom(src => src.TrainingCourses))
                    .ForMember(dst => dst.Documents, x => x.MapFrom(src => src.Documents))
                    .ForMember(dst => dst.ProfilePictureUniqueId, x => x.MapFrom(src => src.EntityImageKey))
                    .ForMember(dst => dst.PasswordHash, x => x.MapFrom(src => src.PasswordHash))
                    .ForMember(dst => dst.EmailConfirmed, x => x.MapFrom(src => src.EmailConfirmed))
                    .ForMember(dst => dst.SecurityStamp, x => x.MapFrom(src => src.SecurityStamp))
                    .ForMember(dst => dst.Role, x => x.MapFrom(src => src.Role))
                    .ForMember(dst => dst.AboutInstructor, x => x.MapFrom(src => src.AboutInstructor))
                    .ForMember(dst => dst.RepresentedUniversityIntroduction, x => x.MapFrom(src => src.RepresentedUniversityIntroduction))
                    .ForMember(dst => dst.UniversityId, x => x.MapFrom(src => src.University.UniversityId))
                    .ForMember(dst => dst.DirectManager, x => x.MapFrom(src => src.DirectManager.ContactId))
                    .ForMember(dst => dst.ArticlesDisclaimer, x => x.MapFrom(src => src.ArticlesDisclaimer))
                    .ForMember(dst => dst.IdeaHubDisclaimer, x => x.MapFrom(src => src.IdeaHubDisclaimer))
                    .ForMember(dst => dst.SpecialProjectDisclaimer, x => x.MapFrom(src => src.SpecialProjectDisclaimer))
                    .ForMember(dst => dst.DeleteAccount, x => x.MapFrom(src => src.DeleteAccount))
                    .ForMember(dst => dst.ProfileUpdateOn, x => x.MapFrom(src => (Date?)src.ProfileUpdateOn))
                    .ReverseMap()
                    .ForMember(src => src.BirthDate, x => x.MapFrom(dst => (Date?)dst.BirthDate))
                    .ForMember(src => src.ProfileUpdateOn, x => x.MapFrom(dst => (Date?)dst.ProfileUpdateOn))
                    .ForMember(src => src.LearningPreferences, x => x.MapFrom(dst => dst.LearningPreferences == null || dst.LearningPreferences.Length == 0 ? null : string.Join(',', dst.LearningPreferences)))
                    .ForMember(src => src.Achievements, x => x.Ignore())
                    .ForMember(src => src.EducationQualifications, x => x.Ignore())
                    .ForMember(src => src.Interests, x => x.Ignore())
                    .ForMember(src => src.LanguageSkills, x => x.Ignore())
                    .ForMember(src => src.Memberships, x => x.Ignore())
                    .ForMember(src => src.IsActiveMember, x => x.MapFrom(src => src.IsActiveMemberInBoardOrInstitution == null ? (int?)null : src.IsActiveMemberInBoardOrInstitution.Value ? 1 : 0))
                    .ForMember(src => src.ProfessionalExperiences, x => x.Ignore())
                    .ForMember(src => src.References, x => x.Ignore())
                    .ForMember(src => src.TrainingCourses, x => x.Ignore())
                    .ForMember(src => src.Documents, x => x.Ignore());
            }
        }
        #endregion
    }
}