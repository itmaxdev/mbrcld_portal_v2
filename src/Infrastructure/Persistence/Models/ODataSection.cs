using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_materialsection")]
    internal sealed class ODataSection
    {
        [DataMember(Name = "do_materialsectionid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_AR { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_order")]
        internal decimal Order { get; set; }

        [DataMember(Name = "do_ModuleMaterialId")]
        internal ODataMaterial Material { get; set; }

        [DataMember(Name = "do_startdate")]
        internal DateTime? StartDate { get; set; }

        [DataMember(Name = "statuscode")]
        internal int SectionStatus { get; set; }

        [DataMember(Name = "do_publishdate")]
        internal DateTime? PublishDate { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSection, Section>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                  .ForMember(dst => dst.MaterialId, x => x.MapFrom(src => src.Material.Id))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.PublishDate, x => x.MapFrom(src => src.PublishDate))
                  .ForMember(dst => dst.SectionStatus, x => x.MapFrom(src => src.SectionStatus))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
