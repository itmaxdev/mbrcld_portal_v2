using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.LanguageSkills.Queries
{
    public class ListUserLanguageSkillsViewModel
    {
        public Guid Id { get; set; }
        public Guid LanguageId { get; set; }
        public int Level { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<LanguageSkill, ListUserLanguageSkillsViewModel>()
                    .ForMember(dst => dst.Level, x => x.MapFrom(src => src.Level))
                    .ForMember(dst => dst.LanguageId, x => x.MapFrom(src => src.LanguageId));
            }
        }
        #endregion
    }
}