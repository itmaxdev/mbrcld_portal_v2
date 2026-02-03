namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class NewsFeed : EntityBase
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
        public decimal Order { get; set; }
        public bool NotifyUsers { get; set; }
        public DateTime? MeetingStartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public Guid ModuleId { get; set; }
        public string ModuleName { get; set; }
        public Guid InstructorId { get; set; }
        public string InstructorName { get; set; }
        public bool Liked { get; set; }
        public int? Likes { get; set; }

        private NewsFeed() { }
        public static NewsFeed Create(
            Guid moduleid,
            Guid instructorid,
            string name,
            int duration,
            decimal order,
            string text,
            int type,
            int status,
            bool notifyusers,
            string url,
            DateTime? meetingstartdate,
            DateTime? expirydate,
            DateTime? publishdate
            )
        {
            Guard.Argument(moduleid, nameof(moduleid)).NotEqual(default);
            Guard.Argument(type, nameof(type)).NotEqual(default);

            var newsfeed = new NewsFeed();
            newsfeed.Id = Guid.NewGuid();
            newsfeed.Name = name;
            newsfeed.Duration = duration;
            newsfeed.Order = order;
            newsfeed.Text = text;
            newsfeed.Type = type;
            newsfeed.Status = status;
            newsfeed.NotifyUsers = notifyusers;
            newsfeed.Url = url;
            newsfeed.ModuleId = moduleid;
            newsfeed.InstructorId = instructorid;
            newsfeed.MeetingStartDate = meetingstartdate;
            newsfeed.ExpiryDate = expirydate;
            newsfeed.PublishDate = publishdate;
            return newsfeed;
        }
    }
}