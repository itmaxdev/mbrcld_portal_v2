using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.NewsFeeds.Commands
{
    public sealed class AddOrEditNewsFeedCommand : IRequest<Result>
    {
        #region Command
        public Guid NewsFeedId { get; }
        public byte[] DocContent { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public decimal Order { get; set; }
        public bool NotifyUsers { get; set; }
        public int Status { get; set; }
        public DateTime? MeetingStartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public Guid ModuleId { get; set; }
        public Guid InstructorId { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditNewsFeedCommand(
            byte[]? doccontent,
            string? contentType,
            string? fileName,
            Guid newsfeedid,
            Guid moduleid,
            Guid instructorid,
            string name,
            int duration,
            int type,
            int status,
            string text,
            string url,
            decimal order,
            bool notifyusers,
            DateTime? meetingstartdate,
            DateTime? publishdate,
            DateTime? expirydate
            )
        {
            this.NewsFeedId = newsfeedid;
            this.ModuleId = moduleid;
            this.InstructorId = instructorid;
            this.DocContent = doccontent;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.Name = name;
            this.Duration = duration;
            this.Order = order;
            this.Text = text;
            this.Type = type;
            this.Status = status;
            this.NotifyUsers = notifyusers;
            this.Url = url;
            this.MeetingStartDate = meetingstartdate;
            this.ExpiryDate = expirydate;
            this.PublishDate = publishdate;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditNewsFeedCommand, Result>
        {
            private readonly IAttachedPictureService attachedPictureService;
            private readonly INewsFeedRepository newsFeedRepository;

            public CommandHandler(IAttachedPictureService attachedPictureService, INewsFeedRepository newsFeedRepository)
            {
                this.attachedPictureService = attachedPictureService;
                this.newsFeedRepository = newsFeedRepository;
            }

            public async Task<Result> Handle(AddOrEditNewsFeedCommand request, CancellationToken cancellationToken)
            {
                var newsfeedid = request.NewsFeedId;
                var newsfeed = await newsFeedRepository.GetNewsFeedByIdAsync(newsfeedid, cancellationToken);
                if (newsfeed.HasValue)
                {
                    var newsfeeddata = newsfeed.Value;
                    newsfeeddata.Name = request.Name;
                    newsfeeddata.Duration = request.Duration;
                    newsfeeddata.ModuleId = request.ModuleId;
                    newsfeeddata.InstructorId = request.InstructorId;
                    newsfeeddata.Text = request.Text;
                    newsfeeddata.Url = request.Url;
                    newsfeeddata.Type = request.Type;
                    newsfeeddata.Status = request.Status;
                    newsfeeddata.NotifyUsers = request.NotifyUsers;
                    newsfeeddata.MeetingStartDate = request.MeetingStartDate;
                    newsfeeddata.ExpiryDate = request.ExpiryDate;
                    newsfeeddata.PublishDate = request.PublishDate;
                    newsfeeddata.Order = request.Order;

                    await newsFeedRepository.UpdateAsync(newsfeeddata).ConfigureAwait(false);
                }
                else
                {
                    var newsfeeddata = NewsFeed.Create(
                        name: request.Name,
                        duration: request.Duration,
                        moduleid: request.ModuleId,
                        instructorid: request.InstructorId,
                        order: request.Order,
                        text: request.Text,
                        url: request.Url,
                        type: request.Type,
                        status: request.Status,
                        notifyusers: request.NotifyUsers,
                        meetingstartdate: request.MeetingStartDate,
                        expirydate: request.ExpiryDate,
                        publishdate: request.PublishDate
                    );

                    var returnednewsfeed = await newsFeedRepository.CreateAsync(newsfeeddata).ConfigureAwait(false);
                    newsfeedid = returnednewsfeed.Value.Id;
                }

                if (request.DocContent != null)
                {
                    return await this.attachedPictureService.AddOrEditNewsfeedDocumentAsync(
                    newsfeedid,
                    request.DocContent,
                    request.ContentType,
                    request.FileName,
                    cancellationToken
                    );
                }

                else
                {
                    return Result.Success(newsfeedid);
                }
            }
        }
        #endregion
    }
}
