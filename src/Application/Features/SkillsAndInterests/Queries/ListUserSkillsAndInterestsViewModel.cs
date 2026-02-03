using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.SkillsAndInterests.Queries
{
    public sealed class ListUserSkillsAndInterestsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Interest, ListUserSkillsAndInterestsViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name));
            }
        }
        #endregion
    }
}
