using AutoMapper;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "annotations")]
    internal sealed class ODataAnnotation
    {
        [DataMember(Name = "annotationid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "subject")]
        internal string Subject { get; set; }

        [DataMember(Name = "filename")]
        internal string FileName { get; set; }

        [DataMember(Name = "mimetype")]
        internal string MimeType { get; set; }

        [DataMember(Name = "_objectid_value")]
        internal Guid Regarding { get; set; }

        [DataMember(Name = "documentbody")]
        internal string DocumentBody { get; set; }

        [DataMember(Name = "filesize")]
        internal int FileSize { get; set; }

        [DataMember(Name = "notetext")]
        internal string Description { get; set; }

        [DataMember(Name = "createdon")]
        internal DateTime CreatedOn { get; set; }

        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataAnnotation, Document>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Identifier, x => x.MapFrom(src => DocumentIdentifier.FromValue(src.Subject).Value))
                    .ForMember(dst => dst.FileName, x => x.MapFrom(src => src.FileName))
                    .ForMember(dst => dst.ContentType, x => x.MapFrom(src => src.MimeType));
            }
        }
    }
}
