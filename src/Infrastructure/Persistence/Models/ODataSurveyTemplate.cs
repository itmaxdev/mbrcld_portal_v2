using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_surveytemplate")]
    internal sealed class ODataSurveyTemplate
    {
        [DataMember(Name = "do_surveytemplateid")]
        internal Guid SurveyTemplateId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_expirydate")]
        internal DateTime ExpiryDate { get; set; }

        [DataMember(Name = "statuscode")]
        public int Status { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSurveyTemplate, SurveyTemplate>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.SurveyTemplateId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.ExpiryDate, x => x.MapFrom(src => src.ExpiryDate))
                    .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
