using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_survey")]
    internal sealed class ODataSurvey
    {
        [DataMember(Name = "do_surveyid")]
        internal Guid SurveyId { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_date")]
        internal DateTime Date { get; set; }

        [DataMember(Name = "do_contactid")]
        internal ODataContact Contact { get; set; }

        [DataMember(Name = "do_surveytemplateid")]
        public ODataSurveyTemplate SurveyTemplate { get; set; }

        [DataMember(Name = "do_ProgramId")]
        public ODataProgram Program { get; set; }

        [DataMember(Name = "statuscode")]
        public int Status { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataSurvey, Survey>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.SurveyId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.SurveyTemplateName, x => x.MapFrom(src => src.SurveyTemplate.Name))
                    .ForMember(dst => dst.Date, x => x.MapFrom(src => src.Date))
                    .ForMember(dst => dst.ContactId, x => x.MapFrom(src => src.Contact.ContactId))
                    .ForMember(dst => dst.ProgramId, x => x.MapFrom(src => src.Program.Id))
                    .ForMember(dst => dst.SurveyTemplateId, x => x.MapFrom(src => src.SurveyTemplate.SurveyTemplateId))
                    .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                    .ReverseMap();
            }
        }
        #endregion
    }
}
