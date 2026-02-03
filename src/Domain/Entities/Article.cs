namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Article : EntityBase
    {
        public string Name { get; set; }
        public int ArticleStatus { get; set; }
        public string Description { get; set; }
        public string TheArticle { get; set; }
        public Guid WrittenBy { get; set; }
        public string WrittenByName { get; set; }
        public DateTime? Date { get; set; }
        public bool Liked { get; set; }
        public bool AdminArticle { get; set; }
        public int? Likes { get; set; }

        private Article() { }
        public static Article Create(
            Guid userid,
            string description,
            string theArticle,
            DateTime? date,
            int articlestatus,
            string name
            )
        {
            Guard.Argument(userid, nameof(userid)).NotEqual(default);

            var article = new Article();
            article.Id = Guid.NewGuid();
            article.WrittenBy = userid;
            article.Description = description;
            article.TheArticle = theArticle;
            article.ArticleStatus = articlestatus;
            article.Date = date;
            article.Name = name;
            return article;
        }
    }
}