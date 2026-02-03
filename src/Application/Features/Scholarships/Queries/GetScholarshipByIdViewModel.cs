using AutoMapper;
using System;
using Mbrcld.Domain.Entities;

namespace Mbrcld.Application.Features.Scholarships.Queries
{
    public sealed class GetScholarshipByIdViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool AlreadyRegistered { get; set; }
        public string PictureUrl { get; set; }
        public string StatusCode { get; set; }

        #region Mapping profile
        public sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Scholarship, GetScholarshipByIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                    .ForMember(dst => dst.FromDate, x => x.MapFrom(src => src.FromDate))
                    .ForMember(dst => dst.StatusCode, x => x.MapFrom(src => src.StatusCode));
            }
        }
        #endregion
    }
}
