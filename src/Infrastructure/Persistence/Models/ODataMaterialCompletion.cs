using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_modulematerialcompletion")]
    internal sealed class ODataMaterialCompletion
    {
        [DataMember(Name = "do_modulematerialcompletionId")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_completed")]
        internal decimal Completed { get; set; }

        [DataMember(Name = "do_ModuleMaterialId")]
        internal ODataMaterial Material { get; set; }

        [DataMember(Name = "do_ContactId")]
        internal ODataContact Contact { get; set; }
        
        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataMaterialCompletion, MaterialCompletion>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.MaterialId, x => x.MapFrom(src => src.Material.Id))
                  .ForMember(dst => dst.ContactID, x => x.MapFrom(src => src.Contact.ContactId))
                  .ForMember(dst => dst.Completed, x => x.MapFrom(src => src.Completed))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
