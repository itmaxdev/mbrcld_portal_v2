using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_specialproject")]
    internal sealed class ODataSpecialProject
    {
        [DataMember(Name = "do_specialprojectid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_AlumniId")]
        internal ODataContact AlumniId { get; set; }

        [DataMember(Name = "do_description")]
        internal string Desription { get; set; }

        [DataMember(Name = "do_body")]
        internal string Body { get; set; }

        [DataMember(Name = "do_specialprojectstatus")]
        internal int? SpecialProjectStatus { get; set; }

        [DataMember(Name = "do_SpecialProjectTopicId")]
        internal ODataSpecialProjectTopic SpecialProjectTopic { get; set; }

        [DataMember(Name = "do_date")]
        internal DateTime? Date { get; set; }

        [DataMember(Name = "do_benchmark")]
        internal string Benchmark { get; set; }

        [DataMember(Name = "do_budget")]
        internal decimal? Budget { get; set; }

        [DataMember(Name = "do_SectorId")]
        internal ODataSector Sector { get; set; }

        [DataMember(Name = "do_othersector")]
        internal string OtherSector { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime? CreatedOn { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSpecialProject, SpecialProject>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.AlumniId, x => x.MapFrom(src => src.AlumniId.ContactId))
                  .ForMember(dst => dst.AlumniName, x => x.MapFrom(src => src.AlumniId.FirstName + " " + src.AlumniId.MiddleName + " " + src.AlumniId.LastName))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Desription))
                  .ForMember(dst => dst.Body, x => x.MapFrom(src => src.Body))
                  .ForMember(dst => dst.SpecialProjectStatus, x => x.MapFrom(src => src.SpecialProjectStatus))
                  .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                  .ForMember(dst => dst.SpecialProjectTopicId, x => x.MapFrom(src => src.SpecialProjectTopic.SpecialProjectTopicId))
                  .ForMember(dst => dst.SectorId, x => x.MapFrom(src => src.Sector.SectorId))
                  .ForMember(dst => dst.OtherSector, x => x.MapFrom(src => src.OtherSector))
                  .ForMember(dst => dst.Benchmark, x => x.MapFrom(src => src.Benchmark))
                  .ForMember(dst => dst.Budget, x => x.MapFrom(src => src.Budget))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
