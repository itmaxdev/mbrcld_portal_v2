using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_cohortcontact")]
    internal sealed class ODataCohortContact
    {
        [DataMember(Name = "do_cohortcontactid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_completed")]
        internal decimal Completed { get; set; }

        [DataMember(Name = "statuscode")]
        internal int Status { get; set; }

        [DataMember(Name = "do_ProgramId")]
        internal ODataProgram Program { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_CohortId")]
        internal ODataCohort Cohort { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataCohortContact, CohortContact>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                  .ForMember(dst => dst.ContactID, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.Completed, x => x.MapFrom(src => src.Completed))
                  .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
