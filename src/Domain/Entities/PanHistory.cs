namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class PanHistory : EntityBase
    {
        public string Name { get; set; }
        public Guid? ArticleId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? NewsFeedId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public int? Action { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Comment { get; set; }

        private PanHistory() { }
        public static PanHistory Create(
            Guid userid,
            Guid? articleid,
            Guid? postid,
            Guid? newsfeedid,
            DateTime actionDate,
            int action,
            string? comment
            )
        {
            Guard.Argument(userid, nameof(userid)).NotEqual(default);
            Guard.Argument(articleid, nameof(articleid)).NotEqual(default);
            Guard.Argument(postid, nameof(postid)).NotEqual(default);
            Guard.Argument(newsfeedid, nameof(newsfeedid)).NotEqual(default);
            Guard.Argument(actionDate, nameof(actionDate)).NotEqual(default);

            var panhistory = new PanHistory();
            panhistory.Id = Guid.NewGuid();
            panhistory.UserId = userid;
            panhistory.ArticleId = articleid;
            panhistory.PostId = postid;
            panhistory.NewsFeedId = newsfeedid;
            panhistory.ActionDate = actionDate;
            panhistory.Action = action;
            panhistory.Comment = comment;
            return panhistory;
        }
    }
}