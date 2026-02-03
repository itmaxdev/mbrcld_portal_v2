using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Services
{
    internal sealed class UserDocumentService : IUserDocumentService
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public UserDocumentService(ISimpleWebApiClient webApiClient, IMapper mapper)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task<Result> InsertOrReplaceDocumentAsync(
            Guid userId,
            DocumentIdentifier documentIdentifier,
            byte[] content,
            string contentType,
            string fileName,
            CancellationToken cancellationToken = default
            )
        {
            var existing = await this.webApiClient.For<AnnotationForRetrieveModel>()
                .Top(1)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => x.AnnotationId)
                .Filter(x => x.ObjectId == userId && x.Subject == documentIdentifier.Value)
                .FindEntriesAsync();

            if (existing.Any())
            {
                var id = (Guid)existing.First().AnnotationId;

                await this.webApiClient.For("annotations")
                    .Key(id)
                    .Set(new
                    {
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                    })
                    .UpdateEntryAsync(false)
                    .ConfigureAwait(false);
            }
            else
            {
                await this.webApiClient.For("annotations")
                    .Set(new
                    {
                        subject = documentIdentifier.Value,
                        filename = fileName,
                        mimetype = contentType,
                        documentbody = Convert.ToBase64String(content),
                        objectid_contact = new { contactid = userId }
                    })
                    .InsertEntryAsync(false)
                    .ConfigureAwait(false);
            }

            return Result.Success();
        }

        public async Task<Result> RemoveDocumentAsync(Guid userId, DocumentIdentifier documentIdentifier, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<AnnotationForRetrieveModel>()
                    .Filter(x => x.ObjectId == userId && x.Subject == documentIdentifier.Value)
                    .Select(x => x.AnnotationId)
                    .DeleteEntriesAsync();

            return Result.Success();
        }

        public async Task<IList<Document>> ListDocumentsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var annotations = await this.webApiClient.For<AnnotationForRetrieveModel>()
                .ProjectToModel()
                .Filter(x => x.ObjectId == userId)
                .FindEntriesAsync();

            return this.mapper.Map<IList<Document>>(annotations);
        }

        [DataContract(Name = "annotations")]
        private sealed class AnnotationForRetrieveModel
        {
            [DataMember(Name = "annotationid")]
            public Guid AnnotationId { get; set; }

            [DataMember(Name = "subject")]
            public string Subject { get; set; }

            [DataMember(Name = "filename")]
            public string FileName { get; set; }

            [DataMember(Name = "mimetype")]
            public string MimeType { get; set; }

            [DataMember(Name = "createdon")]
            public DateTime CreatedOn { get; set; }

            [DataMember(Name = "_objectid_value")]
            public Guid ObjectId { get; set; }

            private sealed class MappingProfile : Profile
            {
                public MappingProfile()
                {
                    CreateMap<AnnotationForRetrieveModel, Document>()
                        .ForMember(dst => dst.Id, x => x.MapFrom(src => src.AnnotationId))
                        .ForMember(dst => dst.Identifier, x => x.MapFrom(src => DocumentIdentifier.FromValue(src.Subject).Value))
                        .ForMember(dst => dst.FileName, x => x.MapFrom(src => src.FileName))
                        .ForMember(dst => dst.ContentType, x => x.MapFrom(src => src.MimeType));
                }
            }
        }
    }
}
