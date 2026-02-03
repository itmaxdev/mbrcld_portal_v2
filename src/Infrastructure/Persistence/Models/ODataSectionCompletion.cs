using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_materialsectioncompletion")]
    internal sealed class ODataSectionCompletion
    {
        [DataMember(Name = "do_materialsectioncompletionid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "statuscode")]
        internal int Status { get; set; }

        [DataMember(Name = "do_MaterialSectionId")]
        internal ODataSection Section { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }
        
        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSectionCompletion, SectionCompletion>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.SectionId, x => x.MapFrom(src => src.Section.Id))
                  .ForMember(dst => dst.ContactID, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
