using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserDisclaimerViewModel
    {
        public Guid Id { get; set; }
        public bool Disclaimer { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, GetUserDisclaimerViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id));
            }
        }
        #endregion
    }
}
