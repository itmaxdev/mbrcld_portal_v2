using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserByPhoneNumberViewModel
    {
        public Guid Id { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, GetUserByPhoneNumberViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id));
            }
        }
        #endregion
    }
}
