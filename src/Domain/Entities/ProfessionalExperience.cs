using Dawn;
using Mbrcld.Domain.Common;
using Mbrcld.SharedKernel.Result;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class ProfessionalExperience : EntityBase
    {
        public Guid Contact { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationName { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public Guid Industry { get; set; }
        public Guid Sector { get; set; }
        public string OtherIndustry { get; set; }
        public string OtherSector { get; set; }
        public string JobTitle_Ar { get; set; }
        public string OrganizationName_Ar { get; set; }
        public int? OrganizationSize { get; set; }
        public int? PositionLevel { get; set; }
        public int? OrganizationLevel { get; set; }
        public bool Completed { get; set; }

        private ProfessionalExperience() {}

        public static ProfessionalExperience Create(
            Guid contact,
            Guid industry,
            Guid sector,
            string organizationName,
            string organizationName_Ar,
            string jobTitle,
            string jobTitle_Ar,
            DateTime from,
            DateTime? to,
            string otherIndustry,
            string otherSector,
            int? organizationSize,
            int? PositionLevel,
            int? OrganizationLevel)
        {
            Guard.Argument(contact, nameof(contact)).NotEqual(default);
            Guard.Argument(industry, nameof(industry)).NotEqual(default);
            Guard.Argument(sector, nameof(sector)).NotEqual(default);
            Guard.Argument(organizationName, nameof(organizationName)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(organizationName_Ar, nameof(organizationName_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(jobTitle, nameof(jobTitle)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(jobTitle_Ar, nameof(jobTitle_Ar)).NotNull().NotEmpty().NotWhiteSpace();
            //Guard.Argument(PositionLevel, nameof(PositionLevel)).NotNull().NotEmpty().NotWhiteSpace();
            //Guard.Argument(OrganizationLevel, nameof(OrganizationLevel)).NotNull().NotEmpty().NotWhiteSpace();

            Guard.Argument(from, nameof(from)).NotEqual(default);

            var professionalExperience = new ProfessionalExperience();
            professionalExperience.Id = Guid.NewGuid();
            professionalExperience.Contact = contact;
            professionalExperience.Industry = industry;
            professionalExperience.Sector = sector;
            professionalExperience.OrganizationName = organizationName;
            professionalExperience.OrganizationName_Ar = organizationName_Ar;
            professionalExperience.JobTitle = jobTitle;
            professionalExperience.JobTitle_Ar = jobTitle_Ar;
            professionalExperience.OtherIndustry = otherIndustry;
            professionalExperience.OtherSector = otherSector;
            professionalExperience.From = from;
            professionalExperience.To = to;
            professionalExperience.OrganizationSize = organizationSize;
            professionalExperience.PositionLevel = PositionLevel; //Enum.GetName(typeof(PositionLevelEnum), PositionLevel);
            professionalExperience.OrganizationLevel = OrganizationLevel;  //Enum.GetName(typeof(OrganizationLevelEnum), OrganizationLevel);
            return professionalExperience;
        }
    }

    //public enum OrganizationLevelEnum
    //{
    //    Country = 0,
    //    Regional = 1,
    //    Global = 3
    //}

    //public enum PositionLevelEnum
    //{
    //    Entity = 0,
    //    Emirate= 1,
    //    Country = 3,
    //    Global = 4
    //}
}
