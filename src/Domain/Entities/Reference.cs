using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class Reference : EntityBase
    {
        public Guid Contact { get; set; }
        public string FullName { get; set; }
        public string FullName_Ar { get; set; }
        public string JobTitle { get; set; }
        public string JobTitle_Ar { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationName_Ar { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsCompleted { get; set; }

        // TODO remove this from domain model, impact on performance
        public string Strengths { get; set; }
        public string Impact { get; set; }
        public string Competency { get; set; }
        public string Capacity { get; set; }
        public string AreasOfImprovement { get; set; }
        public string AdditionalDetails { get; set; }

        private Reference() { }

        public static Reference Create(
            Guid contact,
            string fullName,
            string fullName_Ar,
            string jobTitle,
            string jobTitle_Ar,
            string organizationName,
            string organizationName_Ar,
            string email,
            string mobile
            )
        {
            Guard.Argument(contact, nameof(contact)).NotEqual(default);
            Guard.Argument(fullName, nameof(fullName)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(fullName_Ar, nameof(fullName_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(jobTitle, nameof(jobTitle)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(jobTitle_Ar, nameof(jobTitle_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(jobTitle, nameof(jobTitle)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(jobTitle_Ar, nameof(jobTitle_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(email, nameof(email)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(mobile, nameof(mobile)).NotNull().NotEmpty().NotWhiteSpace();

            var reference = new Reference();
            reference.Id = Guid.NewGuid();
            reference.Contact = contact;
            reference.FullName = fullName;
            reference.FullName_Ar = fullName_Ar;
            reference.OrganizationName = organizationName;
            reference.OrganizationName_Ar = organizationName_Ar;
            reference.JobTitle = jobTitle;
            reference.JobTitle_Ar = jobTitle_Ar;
            reference.Email = email;
            reference.Mobile = mobile;
            return reference;
        }
    }
}
