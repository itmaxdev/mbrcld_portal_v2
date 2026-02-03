using Dawn;
using Mbrcld.Domain.Common;
using Mbrcld.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Mbrcld.Domain.Entities
{
    public sealed class User : EntityBase
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstNameAr { get; set; }
        public string MiddleNameAr { get; set; }
        public string LastNameAr { get; set; }
        public Email Email { get; private set; }
        public Email BusinessEmail { get; set; }
        public Gender Gender { get; set; }
        public string MobilePhone { get; private set; }
        public string MobilePhone2 { get; set; }
        public string Telephone { get; set; }
        public Country Nationality { get; private set; }
        public Country ResidenceCountry { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public string PostOfficeBox { get; set; }
        public string Address { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string EmiratesId { get; set; }
        public string PassportNumber { get; set; }
        public string AboutInstructor { get; set; }
        public string RepresentedUniversityIntroduction { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public int[] LearningPreferences { get; set; }
        public string ProfilePictureUniqueId { get; set; }
        public int? MaritalStatus { get; set; }
        public DateTime? EmiratesIdExpiryDate { get; set; }
        public int? PassportIssuingAuthority { get; set; }
        public bool? IsActiveMemberInBoardOrInstitution { get; set; }
        public int Role { get; set; }
        public Guid? UniversityId { get; set; }
        public Guid? DirectManager { get; set; }
        public bool ArticlesDisclaimer { get; set; }
        public bool IdeaHubDisclaimer { get; set; }
        public bool SpecialProjectDisclaimer{ get; set; }
        public int DeleteAccount { get; set; }
        public DateTime? ProfileUpdateOn { get; set; }

        public IReadOnlyList<Achievement> Achievements { get; private set; }
        public IReadOnlyList<Document> Documents { get; private set; }
        public IReadOnlyList<EducationQualification> EducationQualifications { get; private set; }
        public IReadOnlyList<Interest> Interests { get; private set; }
        public IReadOnlyList<LanguageSkill> LanguageSkills { get; private set; }
        public IReadOnlyList<Membership> Memberships { get; private set; }
        public IReadOnlyList<ProfessionalExperience> ProfessionalExperiences { get; private set; }
        public IReadOnlyList<Reference> References { get; private set; }
        public IReadOnlyList<TrainingCourse> TrainingCourses { get; private set; }
        public IReadOnlyList<EventRegistrant> EventRegistrants { get; private set; }

        #region Identity properties
        public string NormalizedEmail { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecurityStamp { get; set; }
        #endregion

        private User() { }

        public static User Create(
            string firstName,
            string lastName,
            Email email,
            string mobilePhone,
            Country nationality,
            int role,
            bool emailConfirmed
            )
        {
            Guard.Argument(firstName, nameof(firstName)).NotEmpty().NotWhiteSpace();
            Guard.Argument(lastName, nameof(lastName)).NotEmpty().NotWhiteSpace();
            Guard.Argument(email, nameof(email)).NotNull();
            //Guard.Argument(mobilePhone, nameof(mobilePhone)).NotEmpty().NotWhiteSpace();
            Guard.Argument(nationality, nameof(nationality)).NotNull();

            var user = new User();
            user.Id = Guid.NewGuid();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.MobilePhone = mobilePhone;
            user.Nationality = nationality;
            user.Role = role;
            user.EmailConfirmed = emailConfirmed;
            return user;
        }
        
        public void SetEmail(string email)
        {
            this.Email = new Email(email);
        }

        public void SetNormalizedEmail(string normalizedEmail)
        {
            this.NormalizedEmail = normalizedEmail;
        }

        public void SetNationality(Country nationality)
        {
            Guard.Argument(nationality, nameof(nationality)).NotNull();

            this.Nationality = nationality;
        }

        public void SetMobilePhone(string mobilePhone)
        {
            Guard.Argument(mobilePhone, nameof(mobilePhone)).NotNull().NotEmpty().NotWhiteSpace();

            this.MobilePhone = mobilePhone;
        }

        public void SetPasswordHash(string passwordHash)
        {
            this.PasswordHash = passwordHash;
        }

        public void SetEmailConfirmed(bool confirmed)
        {
            this.EmailConfirmed = confirmed;
        }

        public static User SetId(Guid id)
        {
            User user = new User();
            user.Id = id;
            return user;
        }
    }
}
