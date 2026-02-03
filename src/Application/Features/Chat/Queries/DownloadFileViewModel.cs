using AutoMapper;
using ChatData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class DownloadFileViewModel
    {
        public byte[] File { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<File, DownloadFileViewModel>()
                    .ForMember(dst => dst.FileName, x => x.MapFrom(src => src.FileName))
                    .ForMember(dst => dst.ContentType, x => x.MapFrom(src => src.ContentType))
                    .ForMember(dst => dst.Path, x => x.MapFrom(src => src.Path));
            }
        }
        #endregion
    }
}
