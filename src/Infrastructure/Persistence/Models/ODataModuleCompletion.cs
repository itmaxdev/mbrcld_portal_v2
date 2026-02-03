using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_modulecompletion")]
    internal sealed class ODataModuleCompletion
    {
        [DataMember(Name = "do_modulecompletionid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_completed")]
        internal decimal Completed { get; set; }

        [DataMember(Name = "do_ModuleId")]
        internal ODataModule Module { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }
        
        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataModuleCompletion, ModuleCompletion>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.Module.Id))
                  .ForMember(dst => dst.ContactID, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.Completed, x => x.MapFrom(src => src.Completed))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
