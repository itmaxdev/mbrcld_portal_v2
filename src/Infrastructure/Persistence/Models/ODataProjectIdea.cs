using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_projectidea")]
    internal sealed class ODataProjectIdea
    {
        [DataMember(Name = "do_projectideaid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_AlumniId")]
        internal ODataContact AlumniId { get; set; }

        [DataMember(Name = "do_description")]
        internal string Desription { get; set; }

        [DataMember(Name = "do_body")]
        internal string Body { get; set; }

        [DataMember(Name = "do_projectideastatus")]
        internal int? ProjectIdeaStatus { get; set; }

        [DataMember(Name = "do_budget")]
        internal decimal? Budget { get; set; }

        [DataMember(Name = "do_date")]
        internal DateTime? Date { get; set; }

        [DataMember(Name = "do_benchmark")]
        internal string Benchmark { get; set; }

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
                CreateMap<ODataProjectIdea, ProjectIdea>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.AlumniId, x => x.MapFrom(src => src.AlumniId.ContactId))
                  .ForMember(dst => dst.AlumniName, x => x.MapFrom(src => src.AlumniId.FirstName + " " + src.AlumniId.MiddleName + " " + src.AlumniId.LastName))
                  .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Desription))
                  .ForMember(dst => dst.Body, x => x.MapFrom(src => src.Body))
                  .ForMember(dst => dst.ProjectIdeaStatus, x => x.MapFrom(src => src.ProjectIdeaStatus))
                  .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                  .ForMember(dst => dst.Budget, x => x.MapFrom(src => src.Budget))
                  .ForMember(dst => dst.SectorId, x => x.MapFrom(src => src.Sector.SectorId))
                  .ForMember(dst => dst.OtherSector, x => x.MapFrom(src => src.OtherSector))
                  .ForMember(dst => dst.Benchmark, x => x.MapFrom(src => src.Benchmark))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
