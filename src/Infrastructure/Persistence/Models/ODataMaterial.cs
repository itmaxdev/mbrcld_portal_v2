using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_modulematerial")]
    internal sealed class ODataMaterial
    {
        [DataMember(Name = "do_modulematerialid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_name_ar")]
        internal string Name_AR { get; set; }

        [DataMember(Name = "do_location")]
        internal string Location { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_order")]
        internal decimal Order { get; set; }

        [DataMember(Name = "do_ModuleId")]
        internal ODataModule Module { get; set; }

        [DataMember(Name = "do_startdate")]
        internal DateTime? StartDate { get; set; }

        [DataMember(Name = "statuscode")]
        internal int Status { get; set; }

        [DataMember(Name = "do_publishdate")]
        internal DateTime? PublishDate { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataMaterial, Material>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.Name_AR, x => x.MapFrom(src => src.Name_AR))
                  .ForMember(dst => dst.Location, x => x.MapFrom(src => src.Location))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                  .ForMember(dst => dst.StartDate, x => x.MapFrom(src => src.StartDate))
                  .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.Module.Id))
                  .ForMember(dst => dst.PublishDate, x => x.MapFrom(src => src.PublishDate))
                  .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
