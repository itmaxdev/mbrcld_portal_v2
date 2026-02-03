using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class EducationQualification : EntityBase
    {
        public Guid Contact { get; set; }
        public string City { get; set; }
        public string University { get; set; }
        public string University_Ar { get; set; }
        public string Specialization { get; set; }
        public string Specialization_Ar { get; set; }
        public int Degree { get; set; }
        public DateTime Graduationdate { get; set; }
        public Country Country { get; set; }

        private EducationQualification() {}

        public static EducationQualification Create(
            Guid contact,
            string university,
            string university_Ar,
            string specialization,
            string specialization_Ar,
            int degree,
            string city,
            DateTime graduationdate,
            Country country
            )
        {
            Guard.Argument(contact, nameof(contact)).NotEqual(default);
            Guard.Argument(university, nameof(university)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(university_Ar, nameof(university_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(specialization, nameof(specialization)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(specialization_Ar, nameof(specialization_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(degree, nameof(degree)).NotZero().NotNegative();
            Guard.Argument(graduationdate, nameof(graduationdate)).NotEqual(default);

            var educationQualification = new EducationQualification();
            educationQualification.Id = Guid.NewGuid();
            educationQualification.Contact = contact;
            educationQualification.University = university;
            educationQualification.University_Ar = university_Ar;
            educationQualification.Specialization = specialization;
            educationQualification.Specialization_Ar = specialization_Ar;
            educationQualification.Degree = degree;
            educationQualification.Graduationdate = graduationdate;
            educationQualification.Country = country;
            educationQualification.City = city;
            return educationQualification;
        }

        public static EducationQualification SetId(Guid id)
        {
            EducationQualification qualification = new EducationQualification();
            qualification.Id = id;
            return qualification;
        }
    }
}
