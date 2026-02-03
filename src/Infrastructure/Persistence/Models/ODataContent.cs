using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_sectioncontent")]
    internal sealed class ODataContent
    {
        [DataMember(Name = "do_sectioncontentid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_MaterialSectionId")]
        internal ODataSection Section { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_order")]
        internal decimal Order { get; set; }

        [DataMember(Name = "do_meetingstartdate")]
        internal DateTime? StartDate { get; set; }

        [DataMember(Name = "do_type")]
        internal int Type { get; set; }

        [DataMember(Name = "do_url")]
        internal string Url { get; set; }

        [DataMember(Name = "do_text")]
        internal string Text { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataContent, Content>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.SectionId, x => x.MapFrom(src => src.Section.Id))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                  .ForMember(dst => dst.Text, x => x.MapFrom(src => src.Text))
                  .ForMember(dst => dst.Url, x => x.MapFrom(src => src.Url))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
