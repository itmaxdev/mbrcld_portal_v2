using AutoMapper;
using Mbrcld.Domain.Entities;

namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class ListUserDocumentsViewModel
    {
        public string Identifier { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Document, ListUserDocumentsViewModel>()
                    .ForMember(dst => dst.Identifier, x => x.MapFrom(src => src.Identifier == null ? null : src.Identifier.Value))
                    .ForMember(dst => dst.FileName, x => x.MapFrom(src => src.FileName))
                    .ForMember(dst => dst.ContentType, x => x.MapFrom(src => src.ContentType));
            }
        }
        #endregion
    }
}
