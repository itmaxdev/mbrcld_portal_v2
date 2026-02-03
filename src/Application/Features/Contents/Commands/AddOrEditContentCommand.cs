using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class AddOrEditContentCommand : IRequest<Result>
    {
        #region Command
        public Guid ContentId { get; }
        public byte[] DocContent { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal Order { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public Guid SectionId { get; set; }
        public DateTime? StartDate { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditContentCommand(
            byte[]? doccontent,
            string? contentType,
            string? fileName,
            Guid contentId,
            Guid sectionId,
            string name,
            int duration,
            int type,
            string text,
            string url,
            decimal order,
            DateTime? startDate
            )
        {
            this.ContentId = contentId;
            this.SectionId = sectionId;
            this.DocContent = doccontent;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.Name = name;
            this.Duration = duration;
            this.Order = order;
            this.Text = text;
            this.Type = type;
            this.Url = url;
            this.StartDate = startDate;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditContentCommand, Result>
        {
            private readonly IContentRepository contentRepository;
            private readonly IAttachedPictureService attachedPictureService;

            public CommandHandler(IContentRepository contentRepository, IAttachedPictureService attachedPictureService)
            {
                this.contentRepository = contentRepository;
                this.attachedPictureService = attachedPictureService;
            }

            public async Task<Result> Handle(AddOrEditContentCommand request, CancellationToken cancellationToken)
            {
                var contentid = request.ContentId;
                var content = await contentRepository.GetContentByIdAsync(contentid, cancellationToken);
                if (content != null)
                {
                    var contentdata = content;
                    contentdata.Name = request.Name;
                    contentdata.Duration = request.Duration;
                    contentdata.SectionId = request.SectionId;
                    contentdata.Text = request.Text;
                    contentdata.Url = request.Url;
                    contentdata.Type = request.Type;
                    contentdata.StartDate = request.StartDate;
                    contentdata.Order = request.Order;

                    await contentRepository.UpdateAsync(contentdata).ConfigureAwait(false);
                }
                else
                {
                    var contentdata = Content.Create(
                        name: request.Name,
                        duration: request.Duration,
                        order: request.Order,
                        text: request.Text,
                        url: request.Url,
                        type: request.Type,
                        startdate: request.StartDate,
                        sectionid: request.SectionId
                    );

                    var returnedcontent = await contentRepository.CreateAsync(contentdata).ConfigureAwait(false);
                    contentid = returnedcontent.Value.Id;
                }

                if (request.DocContent != null)
                {
                    return await this.attachedPictureService.AddOrEditProgramContentPictureAsync(
                    contentid,
                    request.DocContent,
                    request.ContentType,
                    request.FileName,
                    cancellationToken
                    );
                }

                else
                {
                    return Result.Success(contentid);
                }
            }
        }
        #endregion
    }
}
