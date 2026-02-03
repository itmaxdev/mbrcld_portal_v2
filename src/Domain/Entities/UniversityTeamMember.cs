namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class UniversityTeamMember : EntityBase
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMember { get; set; }
        public string Email { get; set; }
        public Guid? UniversityId { get; set; }
        public string UniversityName { get; set; }
        public Country Nationality { get; set; }
        public string Education { get; set; }
        public string JobPosition { get; set; }
        public string LinkedIn { get; set; }
        public Country ResidenceCountry { get; set; }
        public int status { get; set; }

        private UniversityTeamMember() { }
        public static UniversityTeamMember Create(
            string firstName,
            string lastName,
            string fullname,
            string email,
            string aboutmember,
            Guid? universityid,
            Country nationality,
            Country residencecountry,
            string education,
            string jobposition,
            string linkedin
            )
        {
            Guard.Argument(firstName, nameof(firstName)).NotEmpty().NotWhiteSpace();
            Guard.Argument(lastName, nameof(lastName)).NotEmpty().NotWhiteSpace();
            Guard.Argument(email, nameof(email)).NotNull();
            Guard.Argument(nationality, nameof(nationality)).NotNull();
            Guard.Argument(universityid, nameof(universityid)).NotDefault();

            var universityTeamMember = new UniversityTeamMember();
            universityTeamMember.Id = Guid.NewGuid();
            universityTeamMember.FirstName = firstName;
            universityTeamMember.LastName = lastName;
            universityTeamMember.Name = fullname;
            universityTeamMember.Email = email;
            universityTeamMember.AboutMember = aboutmember;
            universityTeamMember.Nationality = nationality;
            universityTeamMember.ResidenceCountry = residencecountry;
            universityTeamMember.Education = education;
            universityTeamMember.JobPosition = jobposition;
            universityTeamMember.UniversityId = universityid;
            universityTeamMember.LinkedIn = linkedin;
            return universityTeamMember;
        }

        public void SetNationality(Country country)
        {
            Guard.Argument(country, nameof(country)).NotNull();

            this.Nationality = country;
        }

        public void SetResidenceCountry(Country country)
        {
            Guard.Argument(country, nameof(country)).NotNull();

            this.ResidenceCountry = country;
        }
    }
}