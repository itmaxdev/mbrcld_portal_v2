using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.ScholarshipRegistrations.Queries
{
    public sealed class ListUserScholarshipRegistrationViewModel
    {
        public Guid Registrant { get; set; }
        public Guid ScholarshipId { get; set; }
        public string Name { get; set; }
        public string StatusCode { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ScholarshipRegistration, ListUserScholarshipRegistrationViewModel>()
                    .ForMember(dst => dst.Registrant, x => x.MapFrom(src => src.UserId))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.StatusCode, x => x.MapFrom(src => src.StatusCode));
            }
        }
        #endregion
    }
}
