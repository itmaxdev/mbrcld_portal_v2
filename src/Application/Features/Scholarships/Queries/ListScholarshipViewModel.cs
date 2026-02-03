using AutoMapper;
using System;
using Mbrcld.Domain.Entities;

namespace Mbrcld.Application.Features.Scholarships.Queries
{
    public sealed class ListScholarshipViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public bool OpenForRegistration { get; set; }
        public string PictureUrl { get; set; }

        #region Mapping profile
        public sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Scholarship, ListScholarshipViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id.ToString()))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.OpenForRegistration, x => x.MapFrom(src => src.OpenForRegistration));
            }
        }
        #endregion
    }
}
