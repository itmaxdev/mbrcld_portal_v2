using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListUserSurveysUrlViewModel
    {
        public string Name { get; set; }
        public string SurveyTemplateName { get; set; }
        public string SurveyURL { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Survey, ListUserSurveysUrlViewModel>()
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.SurveyTemplateName, x => x.MapFrom(src => src.SurveyTemplateName));
            }
        }
        #endregion
    }
}
